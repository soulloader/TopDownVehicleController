using UnityEngine;

namespace TopDownVehicleController.Camera.Configurations
{
    [CreateAssetMenu(
        fileName = "VehicleCameraConfig",
        menuName = "TopDownVehicleController/Camera/Vehicle Camera Config")]
    public class VehicleCameraConfig : ScriptableObject
    {
        [Header("Follow")]
        public Vector3 followOffset = new Vector3(0f, 12f, -8f);

        [Min(0f)]
        public float followSmoothTime = 0.2f;

        [Header("Look Ahead")]
        [Min(0f)]
        public float lookAheadDistance = 2f;

        [Header("Dynamic Response")]
        [Min(0f)]
        public float speedInfluence = 0.5f;
    }
}