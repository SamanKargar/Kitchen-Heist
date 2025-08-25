using System;
using _Game.Scripts.InteractionSystem;

namespace _Game.Scripts.Events {
    public class MiscEvents {
        public event Action<IInteractable> OnInteractableUpdatedEvent;

        public void OnInteractableUpdated(IInteractable interactable) {
            OnInteractableUpdatedEvent?.Invoke(interactable);
        }
    }
}