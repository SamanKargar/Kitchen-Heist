using System;

namespace _Game.Scripts.Events {
    public class MiscEvents {
        public event Action OnBiscuitPickupEvent;

        public void OnBiscuitPickup() {
            OnBiscuitPickupEvent?.Invoke();
        }
    }
}