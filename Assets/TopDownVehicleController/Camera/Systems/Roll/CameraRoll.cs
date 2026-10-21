using UnityEngine;
using TopDownVehicleController.Camera.Configurations;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Systems.Roll
{
    public sealed class CameraRoll : ICameraSystem
    {
        private CameraContext context;
        private ICameraRollStrategy strategy;

        private float currentRoll;
        private float rollVelocity;

        private Vector3 previousForward;
        private bool initialized;

        public float CurrentRoll => currentRoll;

        public void Initialize(CameraContext context)
        {
            this.context = context;

            currentRoll = 0f;
            rollVelocity = 0f;
            initialized = false;
        }
        public void Reset()
        {
            currentRoll = 0f;
            rollVelocity = 0f;
            initialized = false;
            strategy = null;
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

            var config = context.Configuration;

            if (!config.enableCameraRoll)
            {
                currentRoll = 0f;
                rollVelocity = 0f;
                initialized = false;
                strategy = null;
                return;
            }

            Vector3 forward = targetTransform.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.0001f)
                return;

            forward.Normalize();

            float angularVelocity = 0f;

            if (initialized)
            {
                angularVelocity = Vector3.SignedAngle(
                    previousForward,
                    forward,
                    Vector3.up
                ) / Mathf.Max(Time.deltaTime, 0.0001f);
            }

            previousForward = forward;
            initialized = true;

            bool useMotion = config.cameraRollMode == CameraRollMode.Motion;

            if (strategy == null ||
                (useMotion && !(strategy is MotionRollStrategy)) ||
                (!useMotion && !(strategy is SteeringRollStrategy)))
            {
                strategy = useMotion
                    ? (ICameraRollStrategy)new MotionRollStrategy()
                    : new SteeringRollStrategy();
            }

            float rawRoll = strategy.CalculateRoll(
                context.Target.Velocity,
                angularVelocity
            );

            float targetRoll = Mathf.Clamp(
                rawRoll * config.rollSensitivity,
                -config.maxRollAngle,
                config.maxRollAngle
            );

            currentRoll = Mathf.SmoothDamp(
                currentRoll,
                targetRoll,
                ref rollVelocity,
                config.rollSmoothTime
            );
        }
    }
}