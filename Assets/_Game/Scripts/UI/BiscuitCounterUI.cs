using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class BiscuitCounterUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI counterText;

        private Player _player;

        private void Awake() {
            _player = UtilsClass.GetPlayer();
            counterText.text = $"{_player.GetCollectedBiscuits().ToString()}/{GameManager.Instance.GetRequiredBiscuits()}";
        }

        private void OnEnable() {
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickupEvent += MiscEvents_OnBiscuitPickupEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickupEvent -= MiscEvents_OnBiscuitPickupEvent;
        }

        private void MiscEvents_OnBiscuitPickupEvent() {
            counterText.text = $"{_player.GetCollectedBiscuits().ToString()}/{GameManager.Instance.GetRequiredBiscuits()}";
        }
    }
}