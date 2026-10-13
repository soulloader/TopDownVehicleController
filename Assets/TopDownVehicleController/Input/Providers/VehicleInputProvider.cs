using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using TopDownVehicleController.Input.Interfaces;

namespace TopDownVehicleController.Input.Providers
{
    /// <summary>
    /// Default input provider using Unity Input System.
    /// </summary>
    public sealed class VehicleInputProvider : MonoBehaviour, IVehicleInputProvider
    {
        #region Fields

        private Vector2 move;
        private Vector2 look;
        private bool sprint;

        #endregion

        #region Inspector

        [Header("Movement")]

        [SerializeField]
        private bool analogMovement = true;

        [Header("Cursor")]

        [SerializeField]
        private bool cursorLocked = true;

       
        #endregion

        #region Properties

        public Vector2 Move => move;

        public Vector2 Look => look;

        public bool Sprint => sprint;

        public bool AnalogMovement => analogMovement;

        #endregion

#if ENABLE_INPUT_SYSTEM

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }
        
        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

#endif

        #region Unity

        private void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = cursorLocked
                ? CursorLockMode.Locked
                : CursorLockMode.None;
        }

        #endregion
    }
}