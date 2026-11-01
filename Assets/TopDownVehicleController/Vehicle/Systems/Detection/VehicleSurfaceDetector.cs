using System;
using UnityEngine;

using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Suspension;

namespace TopDownVehicleController.Vehicle.Systems.Detection
{
    /// <summary>
    /// Detects the SurfaceData currently located below the vehicle.
    /// </summary>
    public sealed class VehicleSurfaceDetector :
        MonoBehaviour,
        IVehicleSystem,
        IContextReceiver
    {
        private VehicleContext context;
        private VehicleSuspension suspension;

        [Header("Surface Detection")]

        [SerializeField]
        [Min(0.01f)]
        private float detectionDistance = 3f;

        private SurfaceData currentSurface;

        public SurfaceData CurrentSurface => currentSurface;

        public event Action<SurfaceData> OnSurfaceChanged;

        public void SetContext(VehicleContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            suspension = GetComponent<VehicleSuspension>();
        }

        public void Update()
        {
        }

        public void FixedTick()
        {
            DetectSurface();
        }

        public void LateTick()
        {
        }

        private void DetectSurface()
        {
            if (context == null)
                return;

            SurfaceData detectedSurface = DetectContactSurface();

            if (detectedSurface == null)
                detectedSurface = DetectSurfaceBelow();

            if (detectedSurface == currentSurface)
                return;

            Debug.Log(
                $"[SurfaceDetector] Surface changed to: " +
                $"{(detectedSurface != null ? detectedSurface.SurfaceType.ToString() : "NULL")}",
                this);

            currentSurface = detectedSurface;
            OnSurfaceChanged?.Invoke(currentSurface);
        }

        private SurfaceData DetectContactSurface()
        {
            var contacts = suspension != null ? suspension.SuspensionContacts : null;
            if (contacts == null)
                return null;

            SurfaceData detectedSurface = null;
            float closestDistance = float.MaxValue;

            foreach (var contact in contacts)
            {
                if (!contact.IsGrounded || contact.Collider == null ||
                    contact.Collider.transform.IsChildOf(context.Transform))
                    continue;

                var surface = contact.Collider.GetComponentInParent<SurfaceData>();
                if (surface == null)
                    continue;

                float distance = (contact.Point - context.Transform.position).sqrMagnitude;
                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                detectedSurface = surface;
            }

            return detectedSurface;
        }

        private SurfaceData DetectSurfaceBelow()
        {
            SurfaceData detectedSurface = null;

            // Start above the chassis: a ray starting inside a ramp cannot hit it.
            RaycastHit[] hits = Physics.RaycastAll(
                context.Transform.position + Vector3.up * detectionDistance,
                Vector3.down,
                detectionDistance * 2f,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Ignore);

            float closestDistance = float.MaxValue;

            foreach (RaycastHit hit in hits)
            {
                // Ignore colliders that belong to this vehicle.
                if (hit.transform.IsChildOf(context.Transform))
                    continue;

                SurfaceData surface =
                    hit.collider.GetComponentInParent<SurfaceData>();

                if (surface == null)
                    continue;

                if (hit.distance >= closestDistance)
                    continue;

                closestDistance = hit.distance;
                detectedSurface = surface;
            }

            return detectedSurface;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(
                transform.position + Vector3.up * detectionDistance,
                transform.position +
                Vector3.down * detectionDistance);
        }
    }
}
