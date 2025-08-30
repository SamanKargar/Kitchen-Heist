using _Game.Scripts.InteractionSystem;
using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Items.Interactables {
    public class Interactable_Biscuit : BaseInteractable {
        [SerializeField] private float rotationRate = 7.5f;

        private void Update() {
            transform.Rotate(rotationRate * Time.deltaTime * new Vector3(0f, 10f, 0f));
        }

        public override void Interact() {
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickup();
            Destroy(gameObject);
        }
    }
}