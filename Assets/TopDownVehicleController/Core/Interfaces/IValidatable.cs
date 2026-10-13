namespace TopDownVehicleController.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for systems capable of validating their own configuration.
    /// </summary>
    public interface IValidatable
    {
        bool Validate();
    }
}