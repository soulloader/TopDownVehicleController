using UnityEngine;
using TopDownVehicleController.Input.Interfaces;
using TopDownVehicleController.Vehicle.Interfaces;

namespace TopDownVehicleController.Vehicle.Systems.Audio
{
    /// <summary>
    /// Plays responsive arcade engine audio using three continuously looping layers.
    /// Sprint is audible only while Sprint is held and forward input is positive.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class VehicleEngineAudio : MonoBehaviour, IVehicleSystem, IVehicleInputReceiver
    {
        [Header("Engine Audio Sources")]
        [SerializeField] private AudioSource idleSource;
        [SerializeField] private AudioSource moveSource;
        [SerializeField] private AudioSource sprintSource;

        [Header("Volumes")]
        [SerializeField, Range(0f, 1f)] private float idleVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float moveVolume = 1f;
        [SerializeField, Range(0f, 1f)] private float sprintVolume = 1f;

        [Header("Arcade Transitions")]
        [SerializeField, Min(0f)] private float transitionDuration = 0.08f;
        [SerializeField, Min(0f)] private float inputDeadzone = 0.01f;

        private IVehicleInputProvider inputProvider;
        private bool initialized;

        public void SetInputProvider(IVehicleInputProvider provider)
        {
            inputProvider = provider;
        }

        public void Initialize()
        {
            if (idleSource == null || moveSource == null || sprintSource == null ||
                idleSource == moveSource || idleSource == sprintSource || moveSource == sprintSource)
            {
                Debug.LogError("[TopDownVehicleController] VehicleEngineAudio requires three distinct AudioSources.", this);
                enabled = false;
                return;
            }

            if (inputProvider == null)
            {
                Debug.LogWarning("[TopDownVehicleController] VehicleEngineAudio has no input provider.", this);
                enabled = false;
                return;
            }

            PrepareSource(idleSource);
            PrepareSource(moveSource);
            PrepareSource(sprintSource);

            idleSource.volume = 0f;
            moveSource.volume = 0f;
            sprintSource.volume = 0f;

            // Starting all three loops once avoids restarting clips on every input change.
            idleSource.Play();
            moveSource.Play();
            sprintSource.Play();

            initialized = true;
            UpdateAudio(true);
        }

        public void Update()
        {
            if (initialized)
                UpdateAudio(false);
        }

        public void FixedTick() { }
        public void LateTick() { }

        private static void PrepareSource(AudioSource source)
        {
            source.loop = true;
            source.playOnAwake = false;
        }

        private void UpdateAudio(bool immediate)
        {
            Vector2 move = inputProvider.Move;
            bool hasMovement = move.sqrMagnitude > inputDeadzone * inputDeadzone;
            bool sprintAllowed =
            inputProvider.Sprint && move.sqrMagnitude > inputDeadzone * inputDeadzone;
            float targetIdle = !hasMovement ? idleVolume : 0f;
            float targetMove = hasMovement && !sprintAllowed ? moveVolume : 0f;
            float targetSprint = sprintAllowed ? sprintVolume : 0f;

            // Strict sprint gate: no audible sprint tail after releasing the input
            // or moving backward / stopping forward input.
            if (!sprintAllowed)
                sprintSource.volume = 0f;

            if (immediate || transitionDuration <= 0f)
            {
                idleSource.volume = targetIdle;
                moveSource.volume = targetMove;
                sprintSource.volume = targetSprint;
                return;
            }

            float step = Time.deltaTime / transitionDuration;
            idleSource.volume = Mathf.MoveTowards(idleSource.volume, targetIdle, step);
            moveSource.volume = Mathf.MoveTowards(moveSource.volume, targetMove, step);
            if (sprintAllowed)
                sprintSource.volume = Mathf.MoveTowards(sprintSource.volume, targetSprint, step);
        }

        private void OnDisable()
        {
            initialized = false;
            StopSource(idleSource);
            StopSource(moveSource);
            StopSource(sprintSource);
        }

        private static void StopSource(AudioSource source)
        {
            if (source == null)
                return;

            source.Stop();
            source.volume = 0f;
        }
    }
}
