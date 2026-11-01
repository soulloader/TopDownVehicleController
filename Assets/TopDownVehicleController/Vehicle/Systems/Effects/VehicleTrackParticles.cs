using UnityEngine;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;
using TopDownVehicleController.Vehicle.Systems.Movement;
using TopDownVehicleController.Vehicle.Systems.Suspension;

namespace TopDownVehicleController.Vehicle.Systems.Effects
{
    /// <summary>Controls track emission without clearing particles already alive.</summary>
    [DisallowMultipleComponent]
    public sealed class VehicleTrackParticles : MonoBehaviour, IVehicleSystem
    {
        [SerializeField] private VehicleMovement movement;
        [SerializeField] private VehicleSuspension suspension;
        [SerializeField] private ParticleSystem leftParticles;
        [SerializeField] private ParticleSystem rightParticles;
        [SerializeField] private Transform frontLeft;
        [SerializeField] private Transform backLeft;
        [SerializeField] private Transform frontRight;
        [SerializeField] private Transform backRight;

        public void Initialize()
        {
            if (movement == null) movement = GetComponent<VehicleMovement>();
            if (suspension == null) suspension = GetComponent<VehicleSuspension>();

            SetEmission(leftParticles, false);
            SetEmission(rightParticles, false);

            if (movement == null || suspension == null ||
                leftParticles == null || rightParticles == null ||
                leftParticles == rightParticles || frontLeft == null ||
                backLeft == null || frontRight == null || backRight == null)
            {
                Debug.LogError("[TopDownVehicleController] VehicleTrackParticles is missing valid references.", this);
                enabled = false;
            }
        }

        public void Update() { }
        public void FixedTick() { }

        public void LateTick()
        {
            // Read after all physics systems have updated their state and contacts.
            bool moving = movement != null && movement.isActiveAndEnabled &&
                          movement.CurrentMovementState != VehicleMovementState.Idle;
            bool grounded = suspension != null && suspension.isActiveAndEnabled;
            SetEmission(leftParticles, moving && grounded && HasContact(frontLeft, backLeft));
            SetEmission(rightParticles, moving && grounded && HasContact(frontRight, backRight));
        }

        private bool HasContact(Transform front, Transform back)
        {
            return suspension.TryGetContact(front, out _) ||
                   suspension.TryGetContact(back, out _);
        }

        private static void SetEmission(ParticleSystem particles, bool emitting)
        {
            if (particles == null) return;
            var emission = particles.emission;
            if (emission.enabled != emitting)
                emission.enabled = emitting;
        }

        private void OnDisable()
        {
            SetEmission(leftParticles, false);
            SetEmission(rightParticles, false);
        }
    }
}
