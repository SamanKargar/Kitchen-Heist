using _Game.Scripts.InteractionSystem;
using _Game.Scripts.Managers;

namespace _Game.Scripts.Items.Interactables {
    public class Interactable_Biscuit : BaseInteractable {
        public override void Interact() {
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickup();
            Destroy(gameObject);
        }
    }
}