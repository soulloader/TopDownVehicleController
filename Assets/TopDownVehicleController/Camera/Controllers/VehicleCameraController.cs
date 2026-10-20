using UnityEngine;
using TopDownVehicleController.Camera.Data;
using TopDownVehicleController.Camera.Systems.Follow;
using TopDownVehicleController.Camera.Interfaces;
using TopDownVehicleController.Camera.Configurations;
using TopDownVehicleController.Camera.Systems.LookAhead;

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

        private void Awake()
        {
            target = targetProvider as ICameraTarget;

            if (target == null && targetProvider != null)
            {
                target = targetProvider.GetComponent<ICameraTarget>();
            }

            if (cameraTransform == null ||
                target == null ||
                configuration == null)
            {
                Debug.LogError(
                    "VehicleCameraController: Invalid camera configuration.",
                    this);

                enabled = false;
                return;
            }

            context = new CameraContext(
                cameraTransform,
                target,
                configuration);
            cameraFollow = new CameraFollow();
            cameraFollow.Initialize(context);
            cameraLookAhead = new CameraLookAhead();
            cameraLookAhead.Initialize(context);

            CameraFrame initialFrame = new CameraFrame(
                cameraTransform.position,
                target.TargetTransform.position);

            cameraFollow.LateTick(ref initialFrame);

            cameraTransform.position = initialFrame.Position;

            cameraTransform.rotation = Quaternion.LookRotation(
                -configuration.followOffset.normalized,
                Vector3.up
            );
        }

        private void LateUpdate()
        {
            CameraFrame frame = new CameraFrame(
                cameraTransform.position,
                target.TargetTransform.position
            );

            cameraFollow.LateTick(ref frame);
            cameraLookAhead.LateTick(ref frame);

            cameraTransform.position = Vector3.SmoothDamp(
                cameraTransform.position,
                frame.Position,
                ref followVelocity,
                configuration.followSmoothTime);

            cameraTransform.rotation = Quaternion.LookRotation(
                -configuration.followOffset.normalized, Vector3.up);
        }
    }
}