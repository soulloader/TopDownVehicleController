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
            if (vehicleConfig == null)
            {
                Debug.LogError(
                    "[TopDownVehicleController] VehicleConfig is not assigned.",
                    this);

                enabled = false;
                return;
            }

            if (!CreateContext())
            {
                enabled = false;
                return;
            }

            DiscoverSystems();
            InjectContext();
            InjectInputProvider();
            InjectMovementReference();

            if (!InitializeSystems())
            {
                enabled = false;
                return;
            }

            initialized = true;

        }

        private void Update()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                if (system is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    continue;

                system.Update();
            }
        }

        private void FixedUpdate()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                if (system is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    continue;

                system.FixedTick();
            }
        }

        private void LateUpdate()
        {
            if (!initialized)
                return;

            foreach (IVehicleSystem system in systems)
            {
                if (system is Behaviour behaviour && !behaviour.isActiveAndEnabled)
                    continue;

                system.LateTick();
            }
        }

        #endregion

        #region Initialization

        private bool CreateContext()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Collider col = GetComponent<Collider>();

            if (rb == null || col == null)
            {
                Debug.LogError(
                    "[TopDownVehicleController] Vehicle requires " +
                    "a Rigidbody and a Collider.",
                    this);

                return false;
            }

            context = new VehicleContext(
                transform,
                rb,
                col,
                vehicleConfig);

            return true;
        }

        private void DiscoverSystems()
        {
            systems.Clear();

            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour == null || !behaviour.enabled)
                    continue;

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
                if (behaviour == null)
                    continue;

                if (behaviour is IVehicleInputProvider provider)
                {
                    inputProvider = provider;
                    break;
                }
            }

            if (inputProvider == null)
            {
                Debug.LogWarning(
                    "[TopDownVehicleController] No IVehicleInputProvider found. " +
                    "The vehicle will initialize without an input provider.",
                    this);

                return;
            }

            foreach (IVehicleSystem system in systems)
            {
                if (system is IVehicleInputReceiver receiver)
                {
                    receiver.SetInputProvider(inputProvider);
                }
            }
        }

        private bool InitializeSystems()
        {
            foreach (IVehicleSystem system in systems)
            {
                try
                {
                    system.Initialize();
                }
                catch (System.Exception exception)
                {
                    Debug.LogError(
                        "[TopDownVehicleController] Failed to initialize " +
                        $"vehicle system: {system.GetType().Name}",
                        this);

                    Debug.LogException(exception, this);

                    return false;
                }
            }

            return true;
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