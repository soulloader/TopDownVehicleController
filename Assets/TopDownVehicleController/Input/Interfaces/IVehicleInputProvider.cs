using UnityEngine;

namespace TopDownVehicleController.Input.Interfaces
{
    /// <summary>
    /// Defines the contract for providing vehicle input.
    /// Different implementations may use keyboard,
    /// gamepad, AI, networking or replay.
    /// </summary>
    public interface IVehicleInputProvider
    {
        /// <summary>
        /// Movement input.
        /// </summary>
        Vector2 Move { get; }

        /// <summary>
        /// Look input.
        /// </summary>
        Vector2 Look { get; }

        /// <summary>
        /// Sprint state.
        /// </summary>
        bool Sprint { get; }
    }
}