using System;
using UnityEngine;

namespace _Game.Scripts.Events {
    public class InputEvents {
        public event Action<Vector2> OnMoveActionEvent;
        public event Action<Vector2> OnLookActionEvent;

        public void OnMoveAction(Vector2 inputValue) {
            OnMoveActionEvent?.Invoke(inputValue);
        }

        public void OnLookAction(Vector2 inputValue) {
            OnLookActionEvent?.Invoke(inputValue);
        }
    }
}