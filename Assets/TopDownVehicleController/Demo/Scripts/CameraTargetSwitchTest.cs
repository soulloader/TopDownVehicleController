using UnityEngine;
using UnityEngine.InputSystem;
using TopDownVehicleController.Camera.Controllers;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Demo
{
    public sealed class CameraTargetSwitchTest : MonoBehaviour
    {
        [SerializeField] private VehicleCameraController cameraController;
        [SerializeField] private MonoBehaviour firstTarget;
        [SerializeField] private MonoBehaviour secondTarget;

        private bool useSecondTarget;

        private void Update()
        {
            if (Keyboard.current == null ||
                !Keyboard.current.tKey.wasPressedThisFrame)
            {
                return;
            }

            useSecondTarget = !useSecondTarget;

            MonoBehaviour provider = useSecondTarget
                ? secondTarget
                : firstTarget;

            if (provider == null || cameraController == null)
                return;

            ICameraTarget target = provider as ICameraTarget;

            if (target == null)
            {
                target = provider.GetComponent<ICameraTarget>();
            }

            if (target != null)
            {
                cameraController.SetTarget(target);
            }
        }
    }
}