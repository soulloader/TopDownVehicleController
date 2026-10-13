using UnityEngine;

namespace TopDownVehicleController.Vehicle.Configurations
{
    /// <summary>
    /// Stores the configurable parameters of a vehicle.
    /// This class will evolve as new systems are added to the framework.
    /// </summary>
    [CreateAssetMenu(
        fileName = "VehicleConfig",
        menuName = "TopDown Vehicle Controller/Vehicle Config")]
    public class VehicleConfig : ScriptableObject
    {
        [Header("Movement")]

        [Min(0f)]
        public float moveSpeed = 6f;

        [Min(0f)]
        public float sprintSpeed = 12f;

        [Min(0f)]
        public float moveForce = 40f;
        [Min(0f)]
        public float deceleration = 5f;
        [Range(0f, 20f)]
        public float lateralGrip = 10f;

        [Min(0f)]
        public float rotationLerpSpeed = 8f;
    }
}