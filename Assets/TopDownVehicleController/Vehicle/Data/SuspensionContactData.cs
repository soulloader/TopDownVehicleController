using UnityEngine;

namespace TopDownVehicleController.Vehicle.Data
{
    /// <summary>
    /// Represents a suspension point's ground contact.
    /// </summary>
    public readonly struct SuspensionContactData
    {
        public bool IsGrounded { get; }
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public SuspensionContactData(
            bool isGrounded,
            Vector3 point,
            Vector3 normal)
        {
            IsGrounded = isGrounded;
            Point = point;
            Normal = normal;
        }
    }
}