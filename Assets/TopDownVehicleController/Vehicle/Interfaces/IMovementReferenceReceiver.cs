using UnityEngine;

namespace TopDownVehicleController.Vehicle.Interfaces
{
    /// <summary>
    /// Receives the transform used as the directional reference
    /// for vehicle movement.
    /// </summary>
    public interface IMovementReferenceReceiver
    {
        void SetMovementReference(Transform movementReference);
    }
}