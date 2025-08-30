using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }

        [Tooltip("How many biscuits does this level require to win the game?")]
        [SerializeField] private int requiredBiscuits = 7;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"There's more than one GameManager! {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            UtilsClass.UpdateCursorState(false);
        }

        private void Start() {
            Time.timeScale = 1f;
        }

        #region - Event Handlers -

        private void OnEnable() {
            GameEventsManager.Instance.BehaviorEvents.OnTargetFoundEvent += BehaviorEvents_OnTargetFoundEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.BehaviorEvents.OnTargetFoundEvent -= BehaviorEvents_OnTargetFoundEvent;
        }

        private void BehaviorEvents_OnTargetFoundEvent() {
            GameEventsManager.Instance.GameEvents.OnGameLost();
        }

        #endregion

        public int GetRequiredBiscuits() {
            return requiredBiscuits;
        }
    }
}