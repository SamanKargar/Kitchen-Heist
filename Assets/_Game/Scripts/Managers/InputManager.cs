using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.Managers {
    public class InputManager : MonoBehaviour, PlayerInputActions.IPlayerActions, PlayerInputActions.IUIActions {
        public static InputManager Instance { get; private set; }

        private PlayerInputActions _inputActions;
        
        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"There's more than one InputManager! {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _inputActions = new PlayerInputActions();

            _inputActions.Player.SetCallbacks(this);
            _inputActions.UI.SetCallbacks(this);
            _inputActions.Player.Enable();
        }

        private void OnDisable() {
            _inputActions.Dispose();
        }

        public PlayerInputActions GetInputActions() {
            return _inputActions;
        }

        #region - Player Action Map -

        public void OnMove(InputAction.CallbackContext context) {
            GameEventsManager.Instance.InputEvents.OnMoveAction(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context) {
            GameEventsManager.Instance.InputEvents.OnLookAction(context.ReadValue<Vector2>());
        }

        #endregion

        #region - UI Action Map -

        public void OnNavigate(InputAction.CallbackContext context) {
            
        }

        public void OnSubmit(InputAction.CallbackContext context) {
            
        }

        public void OnCancel(InputAction.CallbackContext context) {
            
        }

        public void OnPoint(InputAction.CallbackContext context) {
            
        }

        public void OnClick(InputAction.CallbackContext context) {
            
        }

        public void OnRightClick(InputAction.CallbackContext context) {
            
        }

        public void OnMiddleClick(InputAction.CallbackContext context) {
            
        }

        public void OnScrollWheel(InputAction.CallbackContext context) {
            
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) {
            
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) {
            
        }

        #endregion
    }
}