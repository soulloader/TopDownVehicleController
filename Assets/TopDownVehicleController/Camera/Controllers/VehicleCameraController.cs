using UnityEngine;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Systems.Follow;
using TopDownVehicleController.Camera.Interfaces;
using TopDownVehicleController.Camera.Configurations;
using TopDownVehicleController.Camera.Systems.LookAhead;
using TopDownVehicleController.Camera.Systems.Dynamics;
using TopDownVehicleController.Camera.Systems.Roll;

namespace TopDownVehicleController.Camera.Controllers
{
    public sealed class VehicleCameraController : MonoBehaviour
    {
        [Header("Camera References")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private MonoBehaviour targetProvider;
        [SerializeField] private VehicleCameraConfig configuration;

        private CameraContext context;
        private ICameraTarget target;
        private CameraFollow cameraFollow;
        private Vector3 followVelocity;
        private CameraLookAhead cameraLookAhead;
        private CameraDynamics cameraDynamics;
        private CameraRoll cameraRoll;

        private void Awake()
        {
            target = targetProvider as ICameraTarget;

            if (target == null && targetProvider != null)
            {
                target = targetProvider.GetComponent<ICameraTarget>();
            }

            if (cameraTransform == null || configuration == null ||
            !HasValidTarget())

            {
                Debug.LogError(
                    "VehicleCameraController: Invalid camera configuration.",
                    this);

                enabled = false;
                return;
            }

            if (configuration.followOffset.sqrMagnitude < 0.0001f)
            {
                Debug.LogError(
                    "VehicleCameraController: Follow offset cannot be zero.",
                    this
                );

                enabled = false;
                return;
            }

            context = new CameraContext(
                cameraTransform,
                target,
                configuration);
            InitializeSystems();

            InitializeCameraPosition();

        }

        private void InitializeSystems()
        {
            cameraFollow = new CameraFollow();
            cameraFollow.Initialize(context);

            cameraLookAhead = new CameraLookAhead();
            cameraLookAhead.Initialize(context);

            cameraDynamics = new CameraDynamics();
            cameraDynamics.Initialize(context);

            cameraRoll = new CameraRoll();
            cameraRoll.Initialize(context);
        }
        private void InitializeCameraPosition()
        {
            CameraFrame initialFrame = new CameraFrame(
                cameraTransform.position,
                target.TargetTransform.position
            );

            cameraFollow.LateTick(ref initialFrame);

            cameraTransform.position = initialFrame.Position;

            cameraTransform.rotation = Quaternion.LookRotation(
                -configuration.followOffset.normalized,
                Vector3.up
            );
        }

        private void LateUpdate()
        {
            if (!HasValidTarget())
            {
                return;
            }

            CameraFrame frame = new CameraFrame(
                cameraTransform.position,
                target.TargetTransform.position
            );

            cameraFollow.LateTick(ref frame);
            cameraLookAhead.LateTick(ref frame);
            cameraDynamics.LateTick(ref frame);
            cameraRoll.LateTick(ref frame);

            ApplyCameraFrame(frame);

        }

        private bool HasValidTarget()
        {
            if (target == null)
                return false;

            if (target is UnityEngine.Object unityTarget &&
                unityTarget == null)
                return false;

            return target.TargetTransform != null;
        }

        private void ApplyCameraFrame(CameraFrame frame)

        {
            cameraTransform.position = Vector3.SmoothDamp(
                cameraTransform.position,
                frame.Position,
                ref followVelocity,
                configuration.followSmoothTime
            );

            Quaternion baseRotation = Quaternion.LookRotation(
                -configuration.followOffset.normalized,
                Vector3.up
            );

            Quaternion rollRotation = Quaternion.Euler(
                0f,
                0f,
                cameraRoll.CurrentRoll
            );

            cameraTransform.rotation = baseRotation * rollRotation;
        }
        public void SetTarget(ICameraTarget newTarget)
        {

            if (context == null || cameraRoll == null)
            {
                Debug.LogWarning(
                    "VehicleCameraController: Camera systems are not initialized.",
                    this
                );
                return;
            }

            if (newTarget == null ||
                (newTarget is UnityEngine.Object unityTarget && unityTarget == null) ||
                newTarget.TargetTransform == null)
            {
                Debug.LogWarning(
                    "VehicleCameraController: Cannot assign an invalid camera target.",
                    this
                );
                return;
            }
            if (ReferenceEquals(target, newTarget))
            {
                return;
            }

            target = newTarget;

            context.SetTarget(newTarget);

            cameraRoll.Reset();
            followVelocity = Vector3.zero;
        }
    }

}