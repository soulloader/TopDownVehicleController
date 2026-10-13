using TopDownVehicleController.Core.Interfaces;

namespace TopDownVehicleController.Vehicle.Interfaces
{
    /// <summary>
    /// Defines the lifecycle contract of a vehicle system.
    /// </summary>
    public interface IVehicleSystem :
        IInitializable,
        IUpdatable,
        IFixedTickable,
        ILateTickable
    {
    }
}