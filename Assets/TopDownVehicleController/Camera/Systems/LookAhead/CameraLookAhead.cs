using UnityEngine;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Systems.LookAhead
{
    public sealed class CameraLookAhead : ICameraSystem
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

            Vector3 velocity = context.Target.Velocity;

            // Keep the look-ahead movement on the horizontal plane.
            velocity.y = 0f;

            float speed = velocity.magnitude;

            if (speed < 0.01f)
                return;

            Vector3 direction = velocity.normalized;

            float influence = Mathf.Clamp01(
                speed * context.Configuration.speedInfluence
            );

            Vector3 lookAheadOffset =
                direction *
                context.Configuration.lookAheadDistance *
                influence;

            frame.Position += lookAheadOffset;
            frame.LookAtPosition += lookAheadOffset;
        }
    }
}