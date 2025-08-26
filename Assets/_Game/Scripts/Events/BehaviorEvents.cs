using System;

namespace _Game.Scripts.Events {
    public class BehaviorEvents {
        public event Action OnTargetFoundEvent;

        public void OnTargetFound() {
            OnTargetFoundEvent?.Invoke();
        }
    }
}