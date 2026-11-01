
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Movement;

namespace TopDownVehicleController.Vehicle.Systems.Suspension
{
    /// <summary>
    /// Handles vehicle suspension, terrain visual alignment,
    /// chassis collision assistance and ramp edge visual tilt.
    /// </summary>
    public sealed class VehicleSuspension : MonoBehaviour, IVehicleSystem, IContextReceiver
    {
        private VehicleContext context;
        private VehicleMovement vehicleMovement;

        [Header("Suspension Points")]
        [SerializeField] private Transform[] suspensionPoints;

        // NEW: Stores the latest ground contact for each suspension point.
        private SuspensionContactData[] suspensionContacts;

        // NEW: Read-only access for other vehicle systems.
        public IReadOnlyList<SuspensionContactData> SuspensionContacts
            => suspensionContacts;

        [Header("Suspension Settings")]
        [SerializeField, Min(0.01f)] private float rayLength = 1.2f;

        [SerializeField]
        [FormerlySerializedAs("groundLayers")]
        [Tooltip("Defines which physics layers can be detected by the suspension. " +
                 "All drivable surfaces must be included in this LayerMask.")]
        private LayerMask suspensionCollisionLayers = 1 << 0;

        [SerializeField, Min(0f)] private float springStrength = 80f;
        [SerializeField, Min(0f)] private float damper = 12f;

        [Header("Vehicle Visual")]
        [SerializeField] private Transform vehicleVisual;

        [Header("Vehicle Visual Calibration")]
        [SerializeField]
        [Tooltip("Adjusts the vehicle model height relative to its root.")]
        private float visualHeightOffset = 0.3f;

        [SerializeField, Min(0f)] private float visualLerpSpeed = 10f;

        [Header("Ramp Edge Visual Assist")]
        [SerializeField, Range(0f, 45f)]
        private float sprintNoseUpAngle = 15f;

        [SerializeField, Range(0f, 45f)]
        private float normalNoseDownAngle = 10f;

        private float currentNoseAngle;

        [Header("Front Collision Assist")]
        [SerializeField] private Collider chassisCollider;
        private bool isFalling;

        [SerializeField] private Transform frontFallCheckPoint;
        [SerializeField, Min(0.01f)] private float frontFallCheckDistance = 1.2f;
        [SerializeField, Min(0.01f)] private float frontFallCheckRadius = 0.15f;

        private Vector3 suspensionGroundNormal;
        private Vector3 frontFallGroundPoint;
        private bool frontFallGroundPointValid;
        private bool frontFallCheckGrounded;
        private float initialGroundDistance;
        private bool visualHeightCalibrated;

        public void SetContext(VehicleContext context)
        {
            this.context = context;
        }

        public bool TryGetContact(Transform suspensionPoint, out SuspensionContactData contact)
        {
            contact = default;

            if (suspensionPoint == null ||
                suspensionPoints == null ||
                suspensionContacts == null)
                return false;

            for (int i = 0; i < suspensionPoints.Length; i++)
            {
                if (suspensionPoints[i] != suspensionPoint)
                    continue;

                if (i >= suspensionContacts.Length)
                    return false;

                contact = suspensionContacts[i];
                return contact.IsGrounded;
            }

            return false;
        }


        public void Initialize()
        {
            // NEW: Allocate contact storage once during initialization.
            suspensionContacts = new SuspensionContactData[
                suspensionPoints != null ? suspensionPoints.Length : 0];

            vehicleMovement = GetComponent<VehicleMovement>();

            // Ensure a valid default collision layer.
            if (suspensionCollisionLayers.value == 0)
            {
                suspensionCollisionLayers = 1 << 0;
            }

            frontFallCheckGrounded = CheckFrontFallGround();
            UpdateChassisCollider();
        }

        public void Update() { }

        public void FixedTick()
        {
            if (context == null || suspensionPoints == null)
                return;

            suspensionGroundNormal = Vector3.zero;

            // UPDATED: Indexed loop to associate each contact
            // with its corresponding suspension point.
            for (int i = 0; i < suspensionPoints.Length; i++)
            {
                Transform point = suspensionPoints[i];

                if (point == null)
                {
                    suspensionContacts[i] = default;
                    continue;
                }

                bool grounded = Physics.Raycast(
                    point.position,
                    -point.up,
                    out RaycastHit hit,
                    rayLength,
                    suspensionCollisionLayers);

                // NEW: Save ground contact without additional raycasts.
                suspensionContacts[i] = grounded ? new SuspensionContactData(true,
                    hit.point, hit.normal, hit.collider) : default;

                if (grounded)
                {
                    suspensionGroundNormal += hit.normal;

                    float compression = (rayLength - hit.distance) / rayLength;
                    Vector3 springForce = point.up * compression * springStrength;
                    Vector3 pointVelocity = context.Rigidbody.GetPointVelocity(point.position);
                    Vector3 damperForce = -pointVelocity * damper;

                    context.Rigidbody.AddForceAtPosition(
                        springForce + damperForce,
                        point.position,
                        ForceMode.Force);
                }
                else
                {
                    context.Rigidbody.AddForceAtPosition(
                        -point.up * 9.81f * 0.5f,
                        point.position,
                        ForceMode.Acceleration);
                }
            }

            frontFallCheckGrounded = CheckFrontFallGround();
            UpdateChassisCollider();

            if (suspensionGroundNormal != Vector3.zero)
                suspensionGroundNormal.Normalize();
            else
                suspensionGroundNormal = Vector3.up;

            UpdateVehicleVisual();
        }

        public void LateTick() { }

        private void UpdateChassisCollider()
        {
            if (chassisCollider == null)
                return;

            // FrontFallCheck exclusively controls chassis collision.
            if (frontFallCheckGrounded)
            {
                chassisCollider.enabled = true;
                isFalling = false;
                currentNoseAngle = 0f;
                return;
            }

            // The front sensor has lost the terrain.
            chassisCollider.enabled = false;

            if (!isFalling)
            {
                isFalling = true;

                bool isSprinting =
                    vehicleMovement != null &&
                    vehicleMovement.CurrentMovementState ==
                    VehicleMovementState.Sprint;

                currentNoseAngle = isSprinting
                    ? -sprintNoseUpAngle
                    : normalNoseDownAngle;
            }
        }

        private void UpdateVehicleVisual()
        {
            if (vehicleVisual == null || context == null)
                return;

            // Update visual height relative to the vehicle root.
            Vector3 targetPosition = vehicleVisual.position;
            float adjustedVisualHeightOffset = visualHeightOffset;

            if (frontFallCheckGrounded && frontFallGroundPointValid && visualHeightCalibrated)
            {
                float distanceDifference =
                    Vector3.Distance(
                        frontFallCheckPoint.position,
                        frontFallGroundPoint) - initialGroundDistance;

                adjustedVisualHeightOffset -= distanceDifference;
            }

            targetPosition.y =
                context.Transform.position.y + adjustedVisualHeightOffset;

            vehicleVisual.position = Vector3.Lerp(
                vehicleVisual.position,
                targetPosition,
                Time.fixedDeltaTime * visualLerpSpeed);

            // Align visual rotation with the average detected terrain normal.
            Vector3 projectedForward = Vector3.ProjectOnPlane(
                context.Transform.forward,
                suspensionGroundNormal);

            if (projectedForward.sqrMagnitude < 0.001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(
                projectedForward,
                suspensionGroundNormal);

            // Apply the captured ramp edge nose tilt.
            targetRotation *= Quaternion.Euler(
                currentNoseAngle,
                0f,
                0f);

            vehicleVisual.rotation = Quaternion.Slerp(
                vehicleVisual.rotation,
                targetRotation,
                Time.fixedDeltaTime * visualLerpSpeed);
        }

        private int GetFrontFallDetectionMask()
        {
            int detectionMask = suspensionCollisionLayers.value;
            int playerLayer = LayerMask.NameToLayer("Player");

            if (playerLayer >= 0)
                detectionMask &= ~(1 << playerLayer);

            return detectionMask;
        }

        private bool CheckFrontFallGround()
        {
            frontFallGroundPointValid = false;

            if (frontFallCheckPoint == null)
                return true;

            // Ground overlap test: the sphere is positioned below the sensor.
            // This overlap alone controls the chassis collider.
            Vector3 checkCenter = frontFallCheckPoint.position +
                                  Vector3.down * frontFallCheckDistance;

            int detectionMask = GetFrontFallDetectionMask();

            bool grounded = Physics.CheckSphere(
                checkCenter,
                frontFallCheckRadius,
                detectionMask,
                QueryTriggerInteraction.Ignore);

            if (grounded)
            {
                // Optional contact point for the existing visual height calibration.
                // This raycast does not control chassis collision.
                if (Physics.Raycast(
                    frontFallCheckPoint.position,
                    Vector3.down,
                    out RaycastHit hit,
                    frontFallCheckDistance + frontFallCheckRadius,
                    detectionMask,
                    QueryTriggerInteraction.Ignore))
                {
                    frontFallGroundPoint = hit.point;
                    frontFallGroundPointValid = true;

                    if (!visualHeightCalibrated)
                    {
                        initialGroundDistance = hit.distance;
                        visualHeightCalibrated = true;
                    }
                }
            }

            return grounded;
        }

        private void OnDisable()
        {
            // Restore chassis collision when the suspension is disabled.
            if (chassisCollider != null)
            {
                chassisCollider.enabled = true;
            }

            // Reset the front fall assist state.
            isFalling = false;
            currentNoseAngle = 0f;
        }

        private void OnDrawGizmosSelected()
        {
            // Keep the suspension ray guides for prefab setup.
            if (suspensionPoints != null)
            {
                foreach (Transform point in suspensionPoints)
                {
                    if (point == null)
                        continue;

                    Gizmos.DrawLine(
                        point.position,
                        point.position - point.up * rayLength);
                }
            }

            if (frontFallCheckPoint != null)
            {
                Vector3 sphereCenter =
                    frontFallCheckPoint.position +
                    Vector3.down * frontFallCheckDistance;

                int detectionMask = GetFrontFallDetectionMask();

                bool groundDetected = Physics.CheckSphere(
                    sphereCenter,
                    frontFallCheckRadius,
                    detectionMask,
                    QueryTriggerInteraction.Ignore);

                // Green = Ground detected.
                // Red = No ground detected.
                Gizmos.color = groundDetected
                    ? Color.green
                    : Color.red;

                Gizmos.DrawWireSphere(
                    sphereCenter,
                    frontFallCheckRadius);

                Gizmos.DrawLine(
                    frontFallCheckPoint.position,
                    sphereCenter);
            }
        }
    }
}
