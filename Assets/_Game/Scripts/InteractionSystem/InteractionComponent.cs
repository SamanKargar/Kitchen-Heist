using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.InteractionSystem {
    public class InteractionComponent {
        private readonly float _interactionRange;
        private readonly float _rayRadius;
        private readonly float _castOffset;
        private readonly Transform _playerTransform;
        private readonly RaycastHit[] _hits;
        
        private IInteractable _currentInteractable;
        
        public InteractionComponent(Transform playerTransform, float castOffset, float interactionRange = 3f, int maxHits = 3) {
            _playerTransform = playerTransform;
            _castOffset = castOffset;
            _interactionRange = interactionRange;
            _hits = new RaycastHit[maxHits];
        }

        public void Update() {
            Ray ray = new Ray(_playerTransform.position + Vector3.up * _castOffset, _playerTransform.forward);
            int hitCount = Physics.RaycastNonAlloc(ray, _hits, _interactionRange);

            _currentInteractable = null;
            
            float closestDist = float.MaxValue;
            for (int i = 0; i < hitCount; i++) {
                RaycastHit hit = _hits[i];
                if (!hit.collider.TryGetComponent(out IInteractable interactable)) continue;
                
                float distance = hit.distance;
                if (!(distance < closestDist)) continue;
                
                closestDist = distance;
                _currentInteractable = interactable;
            }
        }

        public void TryInteract() {
            _currentInteractable?.Interact();
        }

        public IInteractable GetCurrentInteractable() {
            return _currentInteractable;
        }

        public bool HasTarget() {
            return _currentInteractable != null;
        }
    }
}