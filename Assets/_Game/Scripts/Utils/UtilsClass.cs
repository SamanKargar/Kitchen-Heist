using System;
using System.Collections;
using _Game.Scripts.Characters.PlayerCharacter;
using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Utils {
    public static class UtilsClass {
        private class CoroutineRunner : MonoBehaviour { }
        private static CoroutineRunner _runner;

        private static void InitRunner() {
            if (_runner == null) {
                GameObject go = new GameObject("UtilsCoroutineRunner");
                UnityEngine.Object.DontDestroyOnLoad(go);
                _runner = go.AddComponent<CoroutineRunner>();
            }
        }
        
        public static void ExecuteAfterDelay(Action action, float delay) {
            InitRunner();
            _runner.StartCoroutine(ExecuteRoutine(action, delay));
        }

        private static IEnumerator ExecuteRoutine(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        public static void UpdateCursorState(bool value) {
            if (value) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        public static void EnableGameplayActionMap() {
            InputManager.Instance.GetInputActions().Player.Enable();
            InputManager.Instance.GetInputActions().UI.Disable();
        }

        public static void EnableUIActionMap() {
            InputManager.Instance.GetInputActions().UI.Enable();
            InputManager.Instance.GetInputActions().Player.Disable();
        }

        public static Player GetPlayer() {
            return GameObject.FindGameObjectWithTag(PublicConstants.PlayerTag)
                .GetComponent<Player>();
        }

        public static PlayerMovement GetPlayerMovement() {
            return GetPlayer().GetComponent<PlayerMovement>();
        }
    }
}