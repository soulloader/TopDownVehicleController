using UnityEngine;

namespace TopDownVehicleController.Camera.Data
{
    public struct CameraFrame
    {
        public Vector3 Position;
        public Vector3 LookAtPosition;

        public CameraFrame(Vector3 position, Vector3 lookAtPosition)
        {
            Position = position;
            LookAtPosition = lookAtPosition;
        }
    }
}