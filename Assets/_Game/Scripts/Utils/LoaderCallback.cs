using UnityEngine;

namespace _Game.Scripts.Utils {
    public class LoaderCallback : MonoBehaviour {
        private bool _isFirstUpdate = true;

        private void Update() {
            if (_isFirstUpdate) {
                _isFirstUpdate = false;
                Loader.LoaderCallback();
            }
        }
    }
}