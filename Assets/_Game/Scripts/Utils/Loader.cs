using System;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Utils {
    public static class Loader {
        public enum Scene {
            MainMenu,
            LoadingScene,
            Level1,
            Level2,
            Level3,
            Level4
        }

        private static Action _onLoaderCallback;
        
        public static void Load(Scene scene) {
            _onLoaderCallback = () => {
                SceneManager.LoadScene(scene.ToString());
            };

            SceneManager.LoadScene(nameof(Scene.LoadingScene));
        }
        
        public static void LoadNextLevel() {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (Enum.TryParse(currentSceneName, out Scene currentScene)) {
                int nextSceneIndex = (int)currentScene + 1;

                if (Enum.IsDefined(typeof(Scene), nextSceneIndex)) {
                    Load((Scene)nextSceneIndex);
                } else {
                    Load(Scene.MainMenu);
                }
            }
        }
        
        public static void ReloadCurrentLevel() {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (Enum.TryParse(currentSceneName, out Scene currentScene)) {
                Load(currentScene);
            }
        }

        public static void LoaderCallback() {
            if (_onLoaderCallback == null) return;
            
            _onLoaderCallback();
            _onLoaderCallback = null;
        }
    }
}