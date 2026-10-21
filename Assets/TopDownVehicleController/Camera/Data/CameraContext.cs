using UnityEngine;
using TopDownVehicleController.Camera.Configurations;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Data
{
    public sealed class CameraContext
    {
        public Transform CameraTransform { get; }
        public ICameraTarget Target { get; private set; }
        public VehicleCameraConfig Configuration { get; }

        public CameraContext(
            Transform cameraTransform,
            ICameraTarget target,
            VehicleCameraConfig configuration)
        {
            CameraTransform = cameraTransform;
            Target = target;
            Configuration = configuration;
        }

        public void SetTarget(ICameraTarget newTarget)
        {
            Target = newTarget;
        }
    }
}