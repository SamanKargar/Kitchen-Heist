using _Game.Scripts.Characters.PlayerCharacter;
using UnityEngine;

namespace _Game.Scripts.Triggers {
    public class HouseExitTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;

            if (player.IsCarryingBiscuit()) {
                Debug.Log("Game Won!");
            }
        }
    }
}