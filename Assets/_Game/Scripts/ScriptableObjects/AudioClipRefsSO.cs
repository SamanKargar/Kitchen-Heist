using UnityEngine;

namespace _Game.Scripts.ScriptableObjects {
    [CreateAssetMenu(fileName = "New AudioClipRefsSO", menuName = "Kitchen Heist/AudioClipRefs", order = 0)]
    public class AudioClipRefsSO : ScriptableObject {
        [Header("UI Sounds")] [Space(6)]
        
        public AudioClip buttonHoverSound;
        public AudioClip buttonClickSound;
        public AudioClip sliderSound;

        [Space(6)] [Header("Gameplay Sounds")] [Space(6)]
        
        public AudioClip[] footstepSound;
        public AudioClip pickupSound;
    }
}