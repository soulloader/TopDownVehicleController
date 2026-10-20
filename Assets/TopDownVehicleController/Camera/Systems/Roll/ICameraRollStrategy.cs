using UnityEngine;

namespace TopDownVehicleController.Camera.Systems.Roll
{
    public interface ICameraRollStrategy
    {
        float CalculateRoll(
            Vector3 velocity,
            float angularVelocity
        );
    }
}