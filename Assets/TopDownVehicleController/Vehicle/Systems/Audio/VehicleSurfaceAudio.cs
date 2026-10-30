using UnityEngine;
using TopDownVehicleController.Vehicle.Configurations;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Detection;
using TopDownVehicleController.Vehicle.Systems.Movement;

namespace TopDownVehicleController.Vehicle.Systems.Audio
{
    /// <summary>
    /// Plays a looping surface sound while the vehicle is moving.
    /// Changes pitch between Move and Sprint without restarting the clip.
    /// </summary>
    public sealed class VehicleSurfaceAudio : MonoBehaviour, IVehicleSystem, IContextReceiver
    {
        private VehicleContext context;
        private VehicleMovement vehicleMovement;
        private VehicleMovementState movementState = VehicleMovementState.Idle;

        [Header("References")]
        [SerializeField] private VehicleSurfaceDetector surfaceDetector;
        [SerializeField] private AudioSource audioSource;

        [Header("Surface Audio")]
        [SerializeField] private SurfaceAudioConfig[] surfaceAudioConfigs;
        [SerializeField, Min(0f)] private float pitchTransitionDuration = 0.05f;

        private SurfaceAudioConfig activeConfig;
        private float targetPitch = 1f;

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

            if (audioSource == null)
            {
                Debug.LogWarning("[TopDownVehicleController] VehicleSurfaceAudio requires an AudioSource.", this);
                enabled = false;
                return;
            }

            audioSource.playOnAwake = false;
            audioSource.loop = true;
            movementState = vehicleMovement != null
                ? vehicleMovement.CurrentMovementState
                : VehicleMovementState.Idle;

            if (surfaceDetector != null)
                surfaceDetector.OnSurfaceChanged += HandleSurfaceChanged;

            if (vehicleMovement != null)
                vehicleMovement.OnMovementStateChanged += HandleMovementStateChanged;

            RefreshSurfaceAudio(true);
        }

        public void Update()
        {
            if (audioSource == null || activeConfig == null || !audioSource.isPlaying)
                return;

            if (pitchTransitionDuration <= 0f)
            {
                audioSource.pitch = targetPitch;
                return;
            }

            float maxDelta = Mathf.Abs(targetPitch - audioSource.pitch) *
                             Mathf.Min(1f, Time.deltaTime / pitchTransitionDuration);
            // A short, frame-rate-independent exponential-style approach.
            audioSource.pitch = Mathf.MoveTowards(audioSource.pitch, targetPitch, maxDelta);
        }

        public void FixedTick() { }
        public void LateTick() { }

        private void HandleSurfaceChanged(SurfaceData surface)
        {
            RefreshSurfaceAudio(false);
        }

        private void HandleMovementStateChanged(VehicleMovementState newState)
        {
            movementState = newState;
            RefreshSurfaceAudio(false);
        }

        private void RefreshSurfaceAudio(bool immediatePitch)
        {
            if (audioSource == null)
                return;

            if (movementState == VehicleMovementState.Idle || surfaceDetector == null ||
                surfaceDetector.CurrentSurface == null)
            {
                StopSurfaceAudio();
                return;
            }

            SurfaceAudioConfig config = FindConfig(surfaceDetector.CurrentSurface.SurfaceType);
            if (config == null || config.AudioClip == null)
            {
                StopSurfaceAudio();
                return;
            }

            bool clipChanged = audioSource.clip != config.AudioClip;
            activeConfig = config;
            targetPitch = config.Pitch * (movementState == VehicleMovementState.Sprint
                ? config.SprintPitchMultiplier : 1f);

            audioSource.volume = config.Volume;
            audioSource.loop = true;

            if (clipChanged)
            {
                audioSource.clip = config.AudioClip;
                immediatePitch = true;
            }

            if (immediatePitch || pitchTransitionDuration <= 0f)
                audioSource.pitch = targetPitch;

            if (clipChanged || !audioSource.isPlaying)
                audioSource.Play();
        }

        private SurfaceAudioConfig FindConfig(SurfaceType surfaceType)
        {
            if (surfaceAudioConfigs == null)
                return null;

            foreach (SurfaceAudioConfig config in surfaceAudioConfigs)
                if (config != null && config.SurfaceType == surfaceType)
                    return config;

            return null;
        }

        private void StopSurfaceAudio()
        {
            activeConfig = null;
            if (audioSource == null)
                return;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = null;
        }

        private void OnDestroy()
        {
            if (surfaceDetector != null)
                surfaceDetector.OnSurfaceChanged -= HandleSurfaceChanged;

            if (vehicleMovement != null)
                vehicleMovement.OnMovementStateChanged -= HandleMovementStateChanged;
        }
    }
}
