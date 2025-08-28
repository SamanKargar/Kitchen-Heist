using System;
using UnityEngine;

namespace _Game.Scripts.Events {
    public class InputEvents {
        public event Action<Vector2> OnMoveActionEvent;
        public event Action<Vector2> OnLookActionEvent;

        public event Action OnInteractActionEvent;

        public event Action OnJumpActionEvent;

        public event Action OnPauseActionEvent;

        public event Action OnCancelActionEvent;

        public void OnMoveAction(Vector2 inputValue) {
            OnMoveActionEvent?.Invoke(inputValue);
        }

        public void OnLookAction(Vector2 inputValue) {
            OnLookActionEvent?.Invoke(inputValue);
        }

        public void OnInteractAction() {
            OnInteractActionEvent?.Invoke();
        }

        public void OnJumpAction() {
            OnJumpActionEvent?.Invoke();
        }

        public void OnPauseAction() {
            OnPauseActionEvent?.Invoke();
        }

        public void OnCancelAction() {
            OnCancelActionEvent?.Invoke();
        }
    }
}