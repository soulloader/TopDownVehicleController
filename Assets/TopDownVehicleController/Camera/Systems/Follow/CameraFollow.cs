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

            if (context.Target is UnityEngine.Object unityTarget &&
                unityTarget == null)
                return;

            Transform targetTransform = context.Target.TargetTransform;

            if (targetTransform == null)
                return;

            Vector3 targetPosition = targetTransform.position;

            Vector3 desiredPosition =
                targetPosition + context.Configuration.followOffset;

            frame.Position = desiredPosition;
            frame.LookAtPosition = targetPosition;
        }
    }
}