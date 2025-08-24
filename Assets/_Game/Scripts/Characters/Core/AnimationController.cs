using _Game.Scripts.Utils;
using UnityEngine;

namespace _Game.Scripts.Characters.Core {
    public class AnimationController : MonoBehaviour {
        private Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        public void PlayMoveAnimation(float speed) {
            _animator.SetFloat(PublicConstants.SpeedAnimationParameter, speed);
        }
    }
}