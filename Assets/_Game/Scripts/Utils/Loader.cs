using System;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Utils {
    public static class Loader {
        public enum Scene {
            MainMenu,
            LoadingScene,
            PrototypingScene
        }

        private static Action _onLoaderCallback;
        
        public static void Load(Scene scene) {
            _onLoaderCallback = () => {
                SceneManager.LoadScene(scene.ToString());
            };

            SceneManager.LoadScene(nameof(Scene.LoadingScene));
        }

        public static void LoaderCallback() {
            if (_onLoaderCallback == null) return;
            
            _onLoaderCallback();
            _onLoaderCallback = null;
        }
    }
}