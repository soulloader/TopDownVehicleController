namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems that can be reset to their initial state.
    /// </summary>
    public interface IResettable
    {
        void Reset();
    }
}