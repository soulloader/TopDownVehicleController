using TopDownVehicleController.Vehicle.Data;

namespace TopDownVehicleController.Vehicle.Interfaces
{
    /// <summary>
    /// Receives the shared VehicleContext during initialization.
    /// </summary>
    public interface IContextReceiver
    {
        void SetContext(VehicleContext context);
    }
}