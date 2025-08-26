using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private void OnEnable() {
            GameEventsManager.Instance.BehaviorEvents.OnTargetFoundEvent += BehaviorEvents_OnTargetFoundEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.BehaviorEvents.OnTargetFoundEvent -= BehaviorEvents_OnTargetFoundEvent;
        }

        private void BehaviorEvents_OnTargetFoundEvent() {
            Debug.Log("Game Lost!");
            UtilsClass.ExecuteAfterDelay(() => {
                SceneManager.LoadScene(0);
            }, 2f);
        }
    }
}