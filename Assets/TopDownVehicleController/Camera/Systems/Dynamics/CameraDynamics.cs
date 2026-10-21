using UnityEngine;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Systems.Dynamics
{
    public sealed class CameraDynamics : ICameraSystem
    {
        private CameraContext context;
        private float currentZoomOut;
        private float zoomVelocity;

        public void Initialize(CameraContext context)
        {
            this.context = context;
            currentZoomOut = 0f;
            zoomVelocity = 0f;
        }
        public void Reset()
        {
            currentZoomOut = 0f;
            zoomVelocity = 0f;
        }

        public void LateTick(ref CameraFrame frame)
        {
            if (context == null || context.Target == null)
                return;

            var config = context.Configuration;

            Vector3 velocity = context.Target.Velocity;
            velocity.y = 0f;

            float speed = velocity.magnitude;

            float speedFactor = Mathf.Clamp01(
                speed / Mathf.Max(0.1f, config.speedForMaxZoomOut)
            );

            float targetZoomOut =
                speedFactor * config.maxSpeedZoomOut;

            currentZoomOut = Mathf.SmoothDamp(
                currentZoomOut,
                targetZoomOut,
                ref zoomVelocity,
                config.dynamicsSmoothTime
            );

            Vector3 offsetDirection =
                config.followOffset.normalized;

            frame.Position += offsetDirection * currentZoomOut;
        }
    }
}