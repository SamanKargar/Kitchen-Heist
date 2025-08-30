using _Game.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Managers {
    public class SoundManager : MonoBehaviour {
        public static SoundManager Instance { get; private set; }
        
        [SerializeField] private AudioClipRefsSO audioClipRefs;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError($"There's more than one SoundManager! {transform} - {Instance}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable() {
            GameEventsManager.Instance.UIEvents.OnButtonHoverEnterEvent += UIEvents_OnButtonHoverEnterEvent;
            GameEventsManager.Instance.UIEvents.OnButtonHoverExitEvent += UIEvents_OnButtonHoverExitEvent;
            GameEventsManager.Instance.UIEvents.OnButtonClickEvent += UIEvents_OnButtonClickEvent;
            GameEventsManager.Instance.UIEvents.OnSliderValueChangeEvent += UIEvents_OnSliderValueChangeEvent;
            
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickupEvent += MiscEvents_OnBiscuitPickupEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.UIEvents.OnButtonHoverEnterEvent -= UIEvents_OnButtonHoverEnterEvent;
            GameEventsManager.Instance.UIEvents.OnButtonHoverExitEvent -= UIEvents_OnButtonHoverExitEvent;
            GameEventsManager.Instance.UIEvents.OnButtonClickEvent -= UIEvents_OnButtonClickEvent;
            GameEventsManager.Instance.UIEvents.OnSliderValueChangeEvent -= UIEvents_OnSliderValueChangeEvent;
            
            GameEventsManager.Instance.MiscEvents.OnBiscuitPickupEvent -= MiscEvents_OnBiscuitPickupEvent;
        }
        
        private void MiscEvents_OnBiscuitPickupEvent() {
            PlaySound(audioClipRefs.pickupSound, transform.position);
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

        public void PlayFootstepSound(Vector3 position, float volume = 1f) {
            PlaySound(audioClipRefs.footstepSound, position, volume);
        }
        
        private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
            AudioSource.PlayClipAtPoint(audioClip, position, volume);
        }

        private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f) {
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