using System.Collections;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class TypingTextLooper : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField, TextArea] private string fullText = "Loading...";
        [SerializeField] private float typeSpeed = 0.05f;
        [SerializeField] private float pauseBetweenLoops = 1f;

        private void OnEnable() {
            StartCoroutine(TypeLoop());
        }

        private void OnDisable() {
            StopAllCoroutines();
        }

        private IEnumerator TypeLoop() {
            while (true) {
                textMesh.text = "";
                for (int i = 0; i <= fullText.Length; i++) {
                    textMesh.text = fullText[..i];
                    yield return new WaitForSeconds(typeSpeed);
                }

                yield return new WaitForSeconds(pauseBetweenLoops);
            }
        }
    }
}