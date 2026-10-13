using UnityEngine;

using TopDownVehicleController.Vehicle.Configurations;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Detection;
using TopDownVehicleController.Vehicle.Systems.Movement;

namespace TopDownVehicleController.Vehicle.Systems.Audio
{
    /// <summary>
    /// Controls vehicle surface audio based on the
    /// detected surface and vehicle movement state.
    /// </summary>
    public sealed class VehicleSurfaceAudio :
        MonoBehaviour,
        IVehicleSystem,
        IContextReceiver
    {
        private VehicleContext context;
        private VehicleMovement vehicleMovement;

        private VehicleMovementState movementState =
            VehicleMovementState.Idle;

        [Header("References")]

        [SerializeField]
        private VehicleSurfaceDetector surfaceDetector;

        [SerializeField]
        private AudioSource audioSource;

        [Header("Surface Audio")]

        [SerializeField]
        private SurfaceAudioConfig[] surfaceAudioConfigs;

        public void SetContext(VehicleContext context)
        {
            this.context = context;
        }

        public void Initialize()
        {
            if (surfaceDetector == null)
                surfaceDetector = GetComponent<VehicleSurfaceDetector>();

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (vehicleMovement == null)
                vehicleMovement = GetComponent<VehicleMovement>();

            if (surfaceDetector != null)
            {
                surfaceDetector.OnSurfaceChanged +=
                    HandleSurfaceChanged;
            }

            if (vehicleMovement != null)
            {
                vehicleMovement.OnMovementStateChanged +=
                    HandleMovementStateChanged;
            }
        }

        public void Update()
        {
        }

        public void FixedTick()
        {
        }

        public void LateTick()
        {
        }

        private void HandleSurfaceChanged(
            SurfaceData surface)
        {
            if (audioSource == null)
                return;

            if (movementState == VehicleMovementState.Idle)
            {
                StopSurfaceAudio();
                return;
            }

            if (surface == null)
            {
                StopSurfaceAudio();
                return;
            }

            SurfaceAudioConfig config =
                FindConfig(surface.SurfaceType);

            if (config == null ||
                config.AudioClip == null)
            {
                StopSurfaceAudio();
                return;
            }

            audioSource.clip = config.AudioClip;
            audioSource.volume = config.Volume;
            audioSource.pitch = config.Pitch;
            audioSource.loop = true;

            audioSource.Play();
        }

        private void HandleMovementStateChanged(
            VehicleMovementState newState)
        {
            movementState = newState;

            if (movementState == VehicleMovementState.Idle)
            {
                StopSurfaceAudio();
                return;
            }

            if (surfaceDetector != null)
            {
                HandleSurfaceChanged(
                    surfaceDetector.CurrentSurface);
            }
        }

        private SurfaceAudioConfig FindConfig(
            SurfaceType surfaceType)
        {
            if (surfaceAudioConfigs == null)
                return null;

            foreach (SurfaceAudioConfig config
                in surfaceAudioConfigs)
            {
                if (config == null)
                    continue;

                if (config.SurfaceType == surfaceType)
                    return config;
            }

            return null;
        }

        private void StopSurfaceAudio()
        {
            if (audioSource == null)
                return;

            audioSource.Stop();
            audioSource.clip = null;
        }

        private void OnDestroy()
        {
            if (surfaceDetector != null)
            {
                surfaceDetector.OnSurfaceChanged -=
                    HandleSurfaceChanged;
            }

            if (vehicleMovement != null)
            {
                vehicleMovement.OnMovementStateChanged -=
                    HandleMovementStateChanged;
            }
        }
    }
}