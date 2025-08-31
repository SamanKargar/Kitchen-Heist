using UnityEngine;

namespace _Game.Scripts.ScriptableObjects {
    [CreateAssetMenu(fileName = "New AudioClipRefsSO", menuName = "Kitchen Heist/AudioClipRefs", order = 0)]
    public class AudioClipRefsSO : ScriptableObject {
        [Header("UI Sounds")] [Space(6)]
        
        public AudioClip ButtonHoverSound;
        public AudioClip ButtonClickSound;
        public AudioClip ButtonSlideSound;
        public AudioClip SliderSound;

        [Space(6)] [Header("Gameplay Sounds")] [Space(6)]
        
        public AudioClip[] FootstepSound;
        public AudioClip PickupSound;
        public AudioClip WinSound;
        public AudioClip LoseSound;
    }
}