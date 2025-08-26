using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Triggers {
    public class HouseExitTrigger : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other == null || !other.TryGetComponent(out Player player)) return;

            if (player.IsCarryingBiscuit()) {
                player.DisableBiscuitObject();
                Debug.Log("Game Won!");
                UtilsClass.ExecuteAfterDelay(() => {
                    SceneManager.LoadScene(0);
                }, 2f);
            }
        }
    }
}