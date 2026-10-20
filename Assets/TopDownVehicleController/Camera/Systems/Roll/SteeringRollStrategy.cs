using UnityEngine;

namespace TopDownVehicleController.Camera.Systems.Roll
{
    public sealed class SteeringRollStrategy : ICameraRollStrategy
    {
        public float CalculateRoll(
            Vector3 velocity,
            float angularVelocity
        )
        {
            return -angularVelocity;
        }
    }
}