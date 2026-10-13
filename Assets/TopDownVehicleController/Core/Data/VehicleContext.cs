using UnityEngine;
using TopDownVehicleController.Vehicle.Configurations;

namespace TopDownVehicleController.Vehicle.Data
{
    /// <summary>
    /// Contains the shared runtime references used by all vehicle systems.
    /// Built by the VehicleController during initialization.
    /// </summary>
    public class VehicleContext
    {
        /// <summary>
        /// Vehicle transform.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        /// Vehicle rigidbody.
        /// </summary>
        public Rigidbody Rigidbody { get; }

        /// <summary>
        /// Main vehicle collider.
        /// </summary>
        public Collider Collider { get; }

        /// <summary>
        /// Vehicle configuration asset.
        /// </summary>
        public VehicleConfig Config { get; }

        public VehicleContext(
            Transform transform,
            Rigidbody rigidbody,
            Collider collider,
            VehicleConfig config)
        {
            Transform = transform;
            Rigidbody = rigidbody;
            Collider = collider;
            Config = config;
        }
    }
}