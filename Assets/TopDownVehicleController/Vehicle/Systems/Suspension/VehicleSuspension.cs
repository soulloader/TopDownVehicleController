
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
        private bool landingDetected;

        [SerializeField] private Transform frontFallCheckPoint;
        [SerializeField, Min(0.01f)] private float frontFallCheckDistance = 1.2f;

        private Vector3 suspensionGroundNormal;
        private Vector3 frontFallGroundPoint;
        private bool frontFallGroundPointValid;
        private bool frontFallCheckGrounded;
        private float initialGroundDistance;
        private bool visualHeightCalibrated;
        private int groundedFrames;

        public void SetContext(VehicleContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            vehicleMovement = GetComponent<VehicleMovement>();

            frontFallCheckGrounded = CheckFrontFallGround();
            UpdateChassisCollider();
        }

        public void Update() { }

        public void FixedTick()
        {
            if (context == null || suspensionPoints == null)
                return;

            suspensionGroundNormal = Vector3.zero;

            foreach (Transform point in suspensionPoints)
            {
                if (point == null)
                    continue;

                bool grounded = Physics.Raycast(
                    point.position,
                    -point.up,
                    out RaycastHit hit,
                    rayLength,
                    suspensionCollisionLayers);

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

            // Normal grounded state. Losing the FrontFallCheck starts a fall.
            if (!isFalling && !landingDetected)
            {
                if (!frontFallCheckGrounded)
                {
                    isFalling = true;
                    chassisCollider.enabled = false;

                    // Capture movement state at the beginning of the fall.
                    bool isSprinting =
                        vehicleMovement != null &&
                        vehicleMovement.CurrentMovementState ==
                        VehicleMovementState.Sprint;

                    // Negative X raises the nose; positive X lowers it.
                    currentNoseAngle = isSprinting
                        ? -sprintNoseUpAngle
                        : normalNoseDownAngle;
                }

                return;
            }

            // Falling state. Restore the collider when terrain is detected.
            if (isFalling)
            {
                if (frontFallCheckGrounded)
                {
                    chassisCollider.enabled = true;
                    isFalling = false;
                    landingDetected = true;

                    // Return to normal terrain alignment.
                    currentNoseAngle = 0f;
                }

                return;
            }

            // Landing state. Ignore temporary loss of contact after impact.
            if (landingDetected)
            {
                chassisCollider.enabled = true;

                if (frontFallCheckGrounded)
                {
                    groundedFrames++;

                    if (groundedFrames >= 5)
                    {
                        landingDetected = false;
                        groundedFrames = 0;
                    }
                }
                else
                {
                    groundedFrames = 0;
                }

                return;
            }
        }

        private void UpdateVehicleVisual()
        {
            if (vehicleVisual == null || context == null)
                return;

            // Update visual height relative to the vehicle root.
            Vector3 targetPosition = vehicleVisual.position;
            float adjustedVisualHeightOffset = visualHeightOffset;

            if (frontFallCheckGrounded && visualHeightCalibrated)
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

        private bool CheckFrontFallGround()
        {
            frontFallGroundPointValid = false;

            if (frontFallCheckPoint == null)
                return true;

            bool grounded = Physics.Raycast(
                frontFallCheckPoint.position,
                -frontFallCheckPoint.up,
                out RaycastHit hit,
                frontFallCheckDistance,
                suspensionCollisionLayers);

            if (grounded)
            {
                frontFallGroundPoint = hit.point;
                frontFallGroundPointValid = true;

                // Capture the initial ground distance for visual calibration.
                if (!visualHeightCalibrated)
                {
                    initialGroundDistance = hit.distance;
                    visualHeightCalibrated = true;
                }
            }

            return grounded;
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
                Gizmos.DrawLine(
                    frontFallCheckPoint.position,
                    frontFallCheckPoint.position -
                    frontFallCheckPoint.up * frontFallCheckDistance);
            }

            // Visualize only the FrontFallCheck terrain contact.
            if (frontFallGroundPointValid)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(frontFallGroundPoint, 0.12f);
            }
        }
    }
}
