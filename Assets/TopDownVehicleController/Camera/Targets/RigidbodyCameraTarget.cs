using UnityEngine;
using TopDownVehicleController.Camera.Interfaces;

namespace TopDownVehicleController.Camera.Targets
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class RigidbodyCameraTarget : MonoBehaviour, ICameraTarget
    {
        private Rigidbody targetRigidbody;

        public Transform TargetTransform => transform;

        public Vector3 Velocity =>
            targetRigidbody != null
                ? targetRigidbody.velocity
                : Vector3.zero;

        private void Awake()
        {
            targetRigidbody = GetComponent<Rigidbody>();
        }
    }
}