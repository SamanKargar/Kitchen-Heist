using _Game.Scripts.InteractionSystem;
using UnityEngine;

namespace _Game.Scripts.Items.Interactables {
    public class Interactable_Biscuit : BaseInteractable {
        public override void Interact() {
            Debug.Log($"Interacted with: {name}");
        }
    }
}