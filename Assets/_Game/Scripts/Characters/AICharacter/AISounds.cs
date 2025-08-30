using _Game.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.Characters.AICharacter {
    public class AISounds : MonoBehaviour {
        [SerializeField] private float footstepsTimerMax = 0.1f;
        
        private NavMeshAgent _navAgent;
        private float _footstepsTimer;

        private void Awake() {
            _navAgent = GetComponent<NavMeshAgent>();
        }
        
        private void Update() {
            _footstepsTimer -= Time.deltaTime;
            if (_footstepsTimer < 0f) {
                _footstepsTimer = footstepsTimerMax;

                if (_navAgent.velocity.magnitude > 0.1f) {
                    SoundManager.Instance.PlayFootstepSound(_navAgent.transform.position, 0.1f);
                }
            }
        }
    }
}