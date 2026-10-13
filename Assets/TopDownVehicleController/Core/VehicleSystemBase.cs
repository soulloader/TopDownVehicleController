using TopDownVehicleController.Vehicle.Data;

namespace TopDownVehicleController.Vehicle.Interfaces
{
    /// <summary>
    /// Base class for all vehicle systems.
    /// </summary>
    public abstract class VehicleSystemBase :
        IVehicleSystem,
        IContextReceiver
    {
        protected VehicleContext Context { get; private set; }

        public virtual void SetContext(VehicleContext context)
        {
            Context = context;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedTick()
        {
        }

        public virtual void LateTick()
        {
        }
    }
}