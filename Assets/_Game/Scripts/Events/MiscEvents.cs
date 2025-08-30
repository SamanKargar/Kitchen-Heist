using System;

namespace _Game.Scripts.Events {
    public class MiscEvents {
        public event Action OnBiscuitPickupEvent;
        public event Action<int> OnUpdateBiscuitCounterUIEvent;
        
        public event Action OnEnterHidingSpotEvent;
        public event Action OnExitHidingSpotEvent;

        public void OnBiscuitPickup() {
            OnBiscuitPickupEvent?.Invoke();
        }

        public void OnUpdateBiscuitCounterUI(int count) {
            OnUpdateBiscuitCounterUIEvent?.Invoke(count);
        }

        public void OnEnterHidingSpot() {
            OnEnterHidingSpotEvent?.Invoke();
        }

        public void OnExitHidingSpot() {
            OnExitHidingSpotEvent?.Invoke();
        }
    }
}