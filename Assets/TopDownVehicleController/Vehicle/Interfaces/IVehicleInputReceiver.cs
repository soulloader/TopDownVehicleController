using TopDownVehicleController.Input.Interfaces;

namespace TopDownVehicleController.Vehicle.Interfaces
{
    /// <summary>
    /// Receives a vehicle input provider.
    /// </summary>
    public interface IVehicleInputReceiver
    {
        void SetInputProvider(IVehicleInputProvider inputProvider);
    }
}