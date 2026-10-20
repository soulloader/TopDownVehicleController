using UnityEngine;

namespace TopDownVehicleController.Camera.Systems.Roll
{
    public sealed class MotionRollStrategy : ICameraRollStrategy
    {
        private Vector3 previousDirection;
        private bool initialized;

        public float CalculateRoll(
            Vector3 velocity,
            float angularVelocity
        )
        {
            velocity.y = 0f;

            if (velocity.sqrMagnitude < 0.01f)
            {
                initialized = false;
                return 0f;
            }

            Vector3 currentDirection = velocity.normalized;

            if (!initialized)
            {
                previousDirection = currentDirection;
                initialized = true;
                return 0f;
            }

            float angle = Vector3.SignedAngle(
                previousDirection,
                currentDirection,
                Vector3.up
            );

            previousDirection = currentDirection;

            return -angle / Mathf.Max(Time.deltaTime, 0.0001f);
        }
    }
}