using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class PauseMenuUI : MonoBehaviour {
        [SerializeField] private GameObject rootObject;

        private void Awake() {
            HideUI();
        }

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnPauseActionEvent += InputEvents_OnPauseActionEvent;
            GameEventsManager.Instance.InputEvents.OnCancelActionEvent += InputEvents_OnCancelActionEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnPauseActionEvent -= InputEvents_OnPauseActionEvent;
            GameEventsManager.Instance.InputEvents.OnCancelActionEvent -= InputEvents_OnCancelActionEvent;
        }

        private void InputEvents_OnCancelActionEvent() {
            UtilsClass.EnableGameplayActionMap();
            UtilsClass.UpdateCursorState(false);
            
            HideUI();
        }

        private void InputEvents_OnPauseActionEvent() {
            UtilsClass.EnableUIActionMap();
            UtilsClass.UpdateCursorState(true);
            
            ShowUI();
        }

        private void ShowUI() {
            rootObject.SetActive(true);
        }

        private void HideUI() {
            rootObject.SetActive(false);
        }
    }
}