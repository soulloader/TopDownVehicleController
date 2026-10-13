using UnityEngine;

namespace TopDownVehicleController.Vehicle.Data
{
    /// <summary>
    /// Identifies the surface type associated with a scene object.
    /// </summary>
    public sealed class SurfaceData : MonoBehaviour
    {
        [SerializeField]
        private SurfaceType surfaceType = SurfaceType.Ground;

        public SurfaceType SurfaceType => surfaceType;
    }
}