using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.InteractionSystem {
    public abstract class BaseInteractable : MonoBehaviour, IInteractable {
        [Header("Interaction Settings")] [Space(5)]
        [SerializeField] private string interactionPrompt = "Interact";
        [SerializeField] private bool isInteractable = true;

        public bool IsInteractable() {
            return isInteractable;
        }
        
        public string GetInteractionPrompt()
        {
            return interactionPrompt;
        }

        public void UpdateInteractionPrompt(string newPrompt)
        {
            interactionPrompt = newPrompt;
        }

        public virtual void SetInteractable(bool value)
        {
            isInteractable = value;
        }

        public abstract void Interact();

        private void Reset()
        {
            int interactableLayer = LayerMask.NameToLayer(PublicConstants.InteractableLayer);
            if (interactableLayer != -1)
            {
                gameObject.layer = interactableLayer;
            }
            else
            {
                Debug.LogWarning("Layer 'Interactable' was not found! Please add it to the Tags & Layers settings.");
            }
        }
    }
}