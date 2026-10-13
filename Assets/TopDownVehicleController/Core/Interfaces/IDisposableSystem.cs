namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems that release resources during shutdown.
    /// </summary>
    public interface IDisposableSystem
    {
        void Dispose();
    }
}