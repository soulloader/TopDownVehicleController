using UnityEngine;

namespace TopDownVehicleController.Camera.Configurations
{
    public enum CameraRollMode
    {
        Steering,
        Motion
    }
    [CreateAssetMenu(
        fileName = "VehicleCameraConfig",
        menuName = "TopDownVehicleController/Camera/Vehicle Camera Config"
        )]
    public class VehicleCameraConfig : ScriptableObject
    {
        [Header("Follow")]
        public Vector3 followOffset = new Vector3(0f, 12f, -8f);

        [Min(0f)]
        public float followSmoothTime = 0.2f;

        [Header("Look Ahead")]
        [Min(0f)]
        public float lookAheadDistance = 2f;

        [Min(0f)]
        public float speedInfluence = 0.5f;

        [Header("Camera Dynamics")]
        [Min(0f)]
        public float maxSpeedZoomOut = 2f;

        [Min(0.01f)]
        public float dynamicsSmoothTime = 0.3f;

        [Min(0.1f)]
        public float speedForMaxZoomOut = 12f;

        [Header("Camera Roll")]
        public bool enableCameraRoll = false;

        public CameraRollMode cameraRollMode = CameraRollMode.Steering;

        [Range(0f, 15f)]
        public float maxRollAngle = 5f;

        [Min(0.01f)]
        public float rollSmoothTime = 0.2f;

        [Min(0f)]
        public float rollSensitivity = 0.15f;
    }
}