using _Game.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class BiscuitCounterUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI counterText;

        private void Start() {
            counterText.text = $"0/{GameManager.Instance.GetRequiredBiscuits()}";
        }

        private void OnEnable() {
            GameEventsManager.Instance.MiscEvents.OnUpdateBiscuitCounterUIEvent += MiscEvents_OnUpdateBiscuitCounterUIEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.MiscEvents.OnUpdateBiscuitCounterUIEvent -= MiscEvents_OnUpdateBiscuitCounterUIEvent;
        }

        private void MiscEvents_OnUpdateBiscuitCounterUIEvent(int count) {
            counterText.text = $"{count}/{GameManager.Instance.GetRequiredBiscuits()}";
        }
    }
}