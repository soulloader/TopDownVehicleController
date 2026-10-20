using UnityEngine;

namespace TopDownVehicleController.Camera.Interfaces
{
    public interface ICameraTarget
    {
        Transform TargetTransform { get; }

        Vector3 Velocity { get; }
    }
}