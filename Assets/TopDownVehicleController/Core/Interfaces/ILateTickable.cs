namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems that require late frame updates.
    /// </summary>
    public interface ILateTickable
    {
        void LateTick();
    }
}