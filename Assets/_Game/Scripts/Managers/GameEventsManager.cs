using _Game.Scripts.Events;
using UnityEngine;

namespace _Game.Scripts.Managers {
    public class GameEventsManager : MonoBehaviour {
        public static GameEventsManager Instance { get; private set; }

        public InputEvents InputEvents;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Debug.LogError($"There's more than one GameEventsManager! {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InputEvents = new InputEvents();
        }
    }
}