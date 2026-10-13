namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems that require fixed timestep updates.
    /// </summary>
    public interface IFixedTickable
    {
        void FixedTick();
    }
}