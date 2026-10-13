namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems that require one-time initialization.
    /// </summary>
    public interface IInitializable
    {
        void Initialize();
    }
}