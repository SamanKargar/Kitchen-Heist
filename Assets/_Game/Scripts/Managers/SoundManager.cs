using _Game.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game.Scripts.Managers {
    public class SoundManager : MonoBehaviour {
        [SerializeField] private AudioClipRefsSO audioClipRefs;
        
        private void OnEnable() {
            GameEventsManager.Instance.UIEvents.OnButtonHoverEnterEvent += UIEvents_OnButtonHoverEnterEvent;
            GameEventsManager.Instance.UIEvents.OnButtonHoverExitEvent += UIEvents_OnButtonHoverExitEvent;
            GameEventsManager.Instance.UIEvents.OnButtonClickEvent += UIEvents_OnButtonClickEvent;
            GameEventsManager.Instance.UIEvents.OnSliderValueChangeEvent += UIEvents_OnSliderValueChangeEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.UIEvents.OnButtonHoverEnterEvent -= UIEvents_OnButtonHoverEnterEvent;
            GameEventsManager.Instance.UIEvents.OnButtonHoverExitEvent -= UIEvents_OnButtonHoverExitEvent;
            GameEventsManager.Instance.UIEvents.OnButtonClickEvent -= UIEvents_OnButtonClickEvent;
            GameEventsManager.Instance.UIEvents.OnSliderValueChangeEvent -= UIEvents_OnSliderValueChangeEvent;
        }
        
        private void UIEvents_OnSliderValueChangeEvent() {
            PlayUISound(audioClipRefs.sliderSound);
        }
        
        private void UIEvents_OnButtonClickEvent() {
            PlayUISound(audioClipRefs.buttonClickSound);
        }

        private void UIEvents_OnButtonHoverExitEvent() {
            PlayUISound(audioClipRefs.buttonHoverSound);
        }

        private void UIEvents_OnButtonHoverEnterEvent() {
            PlayUISound(audioClipRefs.buttonHoverSound);
        }

        public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        public void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f) {
            PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
        }
        
        private void PlayUISound(AudioClip clip, float volume = 1f) {
            if (clip == null) return;

            GameObject obj = new GameObject("UI_Sound");
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.ignoreListenerPause = true;
            source.Play();

            Destroy(obj, clip.length);
        }
    }
}