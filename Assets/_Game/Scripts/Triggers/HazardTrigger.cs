using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Triggers {
    public class HazardTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other == null || !other.TryGetComponent(out Player _)) return;

            GameEventsManager.Instance.GameEvents.OnGameLost();
        }
    }
}