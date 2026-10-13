using System;
using UnityEngine;

using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;

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

            SurfaceData detectedSurface = null;

            RaycastHit[] hits = Physics.RaycastAll(
                context.Transform.position,
                Vector3.down,
                detectionDistance);

            float closestDistance = float.MaxValue;

            foreach (RaycastHit hit in hits)
            {
                // Ignore colliders that belong to this vehicle.
                if (hit.transform.IsChildOf(context.Transform))
                    continue;

                SurfaceData surface =
                    hit.collider.GetComponent<SurfaceData>();

                if (surface == null)
                    continue;

                if (hit.distance >= closestDistance)
                    continue;

                closestDistance = hit.distance;
                detectedSurface = surface;
            }

            if (detectedSurface == currentSurface)
                return;

            currentSurface = detectedSurface;

            OnSurfaceChanged?.Invoke(currentSurface);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(
                transform.position,
                transform.position +
                Vector3.down * detectionDistance);
        }
    }
}