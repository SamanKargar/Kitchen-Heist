using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Triggers {
    public class HidingSpotTrigger : MonoBehaviour {
        [SerializeField] private GameObject childObject;
        
        private void OnTriggerEnter(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;
            
            int defaultLayer = LayerMask.NameToLayer(PublicConstants.DefaultLayer);
            if (defaultLayer != -1) {
                player.gameObject.layer = defaultLayer;
                GameEventsManager.Instance.MiscEvents.OnEnterHidingSpot();
            }

            int hidingSpotLayer = LayerMask.NameToLayer(PublicConstants.HidingSpotLayer);
            if (hidingSpotLayer != -1) {
                childObject.layer = hidingSpotLayer;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;
            
            int playerLayer = LayerMask.NameToLayer(PublicConstants.PlayerLayer);
            if (playerLayer != -1) {
                player.gameObject.layer = playerLayer;
                GameEventsManager.Instance.MiscEvents.OnExitHidingSpot();
            }

            childObject.layer = LayerMask.NameToLayer(PublicConstants.DefaultLayer);
        }
    }
}