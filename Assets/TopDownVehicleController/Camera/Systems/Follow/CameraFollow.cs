using UnityEngine;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Systems.Follow
{
    public sealed class CameraFollow : ICameraSystem
    {
        private CameraContext context;

        public void Initialize(CameraContext context)
        {
            this.context = context;
        }

        public void LateTick(ref CameraFrame frame)
        {
            if (context == null || context.Target == null)
                return;

            Vector3 targetPosition =
                context.Target.TargetTransform.position;

            Vector3 desiredPosition =
                targetPosition + context.Configuration.followOffset;

            frame.Position = desiredPosition;
            frame.LookAtPosition = targetPosition;
        }
    }
}