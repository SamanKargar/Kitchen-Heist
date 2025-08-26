using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Triggers {
    public class HidingSpotTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;
            
            int defaultLayer = LayerMask.NameToLayer(PublicConstants.DefaultLayer);
            if (defaultLayer != -1) {
                player.gameObject.layer = defaultLayer;
                GameEventsManager.Instance.MiscEvents.OnEnterHidingSpot();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;
            
            int playerLayer = LayerMask.NameToLayer(PublicConstants.PlayerLayer);
            if (playerLayer != -1) {
                player.gameObject.layer = playerLayer;
                GameEventsManager.Instance.MiscEvents.OnExitHidingSpot();
            }
        }
    }
}