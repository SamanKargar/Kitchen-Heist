using _Game.Scripts.InteractionSystem;
using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Characters.PlayerCharacter {
    public class Player : MonoBehaviour {
        [Header("Interaction Settings")] [Space(6)]
        
        [SerializeField] private float interactionRange = 0.5f;
        [SerializeField] private float castOffset = 0.225f;

        private float _interactionUpdateTimer;
        
        private InteractionComponent _interactionComponent;

        private void Awake() {
            _interactionComponent = new InteractionComponent(transform, castOffset, interactionRange);
        }

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnInteractActionEvent += InputEvents_OnInteractActionEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnInteractActionEvent -= InputEvents_OnInteractActionEvent;
        }
        
        private void InputEvents_OnInteractActionEvent() {
            if (!_interactionComponent.HasTarget()) return;
            
            _interactionComponent.TryInteract();
        }

        private void Update() {
            InteractionComponentUpdate();
        }

        private void InteractionComponentUpdate() {
            _interactionUpdateTimer -= Time.deltaTime;
            
            if (!(_interactionUpdateTimer <= 0f)) return;
            
            const float INTERACTION_UPDATE_TIMER_MAX = 0.2f;
            _interactionUpdateTimer += INTERACTION_UPDATE_TIMER_MAX;
            
            _interactionComponent.Update();
        }
    }
}