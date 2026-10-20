using TopDownVehicleController.Camera.Data;

namespace TopDownVehicleController.Camera.Interfaces
{
    public interface ICameraSystem
    {
        void Initialize(CameraContext context);

        void LateTick(ref CameraFrame frame);
    }
}