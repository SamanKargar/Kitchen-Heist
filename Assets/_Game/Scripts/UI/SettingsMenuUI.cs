using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class SettingsMenuUI : MonoBehaviour {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private AudioMixer mainAudioMixer;
        
        public void SetFullscreen(bool isFullscreen) {
            Screen.fullScreen = isFullscreen;
        }

        public void SetMasterVolume() {
            SetVolume(PublicConstants.MasterVolumeMixer, masterVolumeSlider.value);
        }

        public void SetMusicVolume() {
            SetVolume(PublicConstants.MusicVolumeMixer, musicVolumeSlider.value);
        }

        public void SetSFXVolume() {
            SetVolume(PublicConstants.SFXVolumeMixer, sfxVolumeSlider.value);
        }
        
        private void SetVolume(string parameterName, float value) {
            float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
            mainAudioMixer.SetFloat(parameterName, dB);
        }
    }
}