using System;
using UnityEngine;
using TopDownVehicleController.Input.Interfaces;
using TopDownVehicleController.Vehicle.Data;
using TopDownVehicleController.Vehicle.Interfaces;


namespace TopDownVehicleController.Vehicle.Systems.Movement
{
    /// <summary>
    /// Handles vehicle movement behaviour.
    /// </summary>
    public sealed class VehicleMovement :
     MonoBehaviour,
     IVehicleSystem,
     IContextReceiver,
     IVehicleInputReceiver,
     IMovementReferenceReceiver
    {
        private VehicleContext context;
        private IVehicleInputProvider inputProvider;
        private Transform movementReference;
        private VehicleMovementState movementState = VehicleMovementState.Idle;

        private VehicleMovement vehicleMovement;

        public event Action<VehicleMovementState> OnMovementStateChanged;
        public VehicleMovementState CurrentMovementState => movementState;

        public void SetContext(VehicleContext context)
        {
            this.context = context;
        }

        public void SetInputProvider(IVehicleInputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }

        public void SetMovementReference(Transform movementReference)
        {
            this.movementReference = movementReference;
        }

        public void Initialize()
        {

        }

        public void Update()
        {
        }
        public void FixedTick()
        {
            if (context == null || inputProvider == null || context.Config == null)
                return;

            Vector2 input = inputProvider.Move;

            if (input == Vector2.zero)
            {
                SetMovementState(VehicleMovementState.Idle);

                ApplyDeceleration();
                ApplyLateralGrip();
                return;
            }

            SetMovementState(inputProvider.Sprint
                ? VehicleMovementState.Sprint
                : VehicleMovementState.Move);

            Vector3 moveDirection;

            if (movementReference != null)
            {
                Vector3 forward = movementReference.forward;
                Vector3 right = movementReference.right;

                forward.y = 0f;
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                moveDirection =
                    (forward * input.y +
                     right * input.x).normalized;
            }
            else
            {
                moveDirection = new Vector3(
                    input.x,
                    0f,
                    input.y).normalized;
            }

            float targetRotation =
                Mathf.Atan2(moveDirection.x, moveDirection.z) *
                Mathf.Rad2Deg;

            Quaternion desiredRotation =
                Quaternion.Euler(0f, targetRotation, 0f);

            context.Rigidbody.rotation = Quaternion.Slerp(
                context.Rigidbody.rotation,
                desiredRotation,
                Time.fixedDeltaTime * context.Config.rotationLerpSpeed);

            float targetSpeed = inputProvider.Sprint
                ? context.Config.sprintSpeed
                : context.Config.moveSpeed;

            Vector3 desiredVelocity =
                moveDirection * targetSpeed;

            Vector3 currentHorizontalVelocity =
                new Vector3(
                    context.Rigidbody.velocity.x,
                    0f,
                    context.Rigidbody.velocity.z);

            Vector3 velocityDifference =
                desiredVelocity - currentHorizontalVelocity;

            Vector3 acceleration =
                velocityDifference *
                context.Config.moveForce *
                Time.fixedDeltaTime;

            context.Rigidbody.AddForce(
                acceleration,
                ForceMode.VelocityChange);
            ApplyLateralGrip();
        }

        private void SetMovementState(VehicleMovementState newState)
        {
            if (movementState == newState)
                return;

            movementState = newState;
            OnMovementStateChanged?.Invoke(movementState);
        }

        private void ApplyDeceleration()
        {
            Vector3 velocity = context.Rigidbody.velocity;

            Vector3 horizontalVelocity = new Vector3(
                velocity.x,
                0f,
                velocity.z);

            horizontalVelocity = Vector3.MoveTowards(
                horizontalVelocity,
                Vector3.zero,
                context.Config.deceleration * Time.fixedDeltaTime);

            context.Rigidbody.velocity = new Vector3(
                horizontalVelocity.x,
                velocity.y,
                horizontalVelocity.z);
        }
        private void ApplyLateralGrip()
        {
            if (context.Config.lateralGrip <= 0f)
                return;

            Vector3 localVelocity =
                context.Transform.InverseTransformDirection(
                    context.Rigidbody.velocity);

            localVelocity.x = Mathf.MoveTowards(
                localVelocity.x,
                0f,
                context.Config.lateralGrip * Time.fixedDeltaTime);

            context.Rigidbody.velocity =
                context.Transform.TransformDirection(localVelocity);
        }
        public void LateTick()
        {
        }
    }
}