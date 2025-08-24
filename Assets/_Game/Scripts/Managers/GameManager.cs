using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"There's more than one GameManager! {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            UtilsClass.UpdateCursorState(false);
        }
    }
}