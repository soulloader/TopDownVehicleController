using System.Collections.Generic;
using UnityEngine;
using TopDownVehicleController.Input.Interfaces;
using TopDownVehicleController.Vehicle.Configurations;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;


namespace TopDownVehicleController.Vehicle
{
    /// <summary>
    /// Central coordinator of the Vehicle Framework.
    /// Responsible only for creating the VehicleContext,
    /// discovering systems and executing their lifecycle.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public sealed class VehicleController : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private VehicleConfig vehicleConfig;
        [SerializeField]
        private Transform movementReference;

        #endregion

        #region Fields

        private VehicleContext context;

        private readonly List<IVehicleSystem> systems = new();

        private bool initialized;

        #endregion

        #region Unity

        private void Awake()
        {
            CreateContext();
            DiscoverSystems();
            InjectContext();
            InjectInputProvider();
            InjectMovementReference();
            InitializeSystems();

            initialized = true;
        }

        private void Update()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                system.Update();
            }
        }

        private void FixedUpdate()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                system.FixedTick();
            }
        }

        private void LateUpdate()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                system.LateTick();
            }
        }

        #endregion

        #region Initialization

        private void CreateContext()
        {
            context = new VehicleContext(
                transform,
                GetComponent<Rigidbody>(),
                GetComponent<Collider>(),
                vehicleConfig);
        }

        private void DiscoverSystems()
        {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is IVehicleSystem system)
                {
                    systems.Add(system);
                }
            }
        }

        private void InjectContext()
        {
            foreach (IVehicleSystem system in systems)
            {
                if (system is IContextReceiver receiver)
                {
                    receiver.SetContext(context);
                }
            }
        }

        private void InjectInputProvider()
        {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();

            IVehicleInputProvider inputProvider = null;

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is IVehicleInputProvider provider)
                {
                    inputProvider = provider;
                    break;
                }
            }

            if (inputProvider == null)
                return;

            foreach (IVehicleSystem system in systems)
            {
                if (system is IVehicleInputReceiver receiver)
                {
                    receiver.SetInputProvider(inputProvider);
                }
            }
        }

        private void InitializeSystems()
        {
            foreach (IVehicleSystem system in systems)
            {
                system.Initialize();
            }
        }

        private void InjectMovementReference()
        {
            if (movementReference == null)
                return;

            foreach (IVehicleSystem system in systems)
            {
                if (system is IMovementReferenceReceiver receiver)
                {
                    receiver.SetMovementReference(movementReference);
                }
            }
        }

        #endregion
    }
}