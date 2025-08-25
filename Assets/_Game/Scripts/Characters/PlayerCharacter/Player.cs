using _Game.Scripts.InteractionSystem;
using _Game.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.Characters.PlayerCharacter {
    public class Player : MonoBehaviour {
        [Header("Interaction Settings")] [Space(6)]
        
        [SerializeField] private GameObject interactionPromptRoot;
        [SerializeField] private TextMeshProUGUI interactionText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float interactionRange = 0.5f;
        [SerializeField] private float castOffset = 0.225f;
        [SerializeField] private float animationDuration = 0.75f;

        private float _interactionUpdateTimer;
        
        private InteractionComponent _interactionComponent;
        private Tween _fadeTween;

        private void Awake() {
            _interactionComponent = new InteractionComponent(transform, castOffset, interactionRange);
            canvasGroup.alpha = 0f;
            interactionPromptRoot.SetActive(false);
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
            InteractionUIUpdate();
        }

        private void InteractionUIUpdate() {
            if (_interactionComponent.HasTarget()) {
                ShowUI();
            }
            else {
                HideUI();
            }
        }

        private void ShowUI() {
            interactionPromptRoot.SetActive(true);
            interactionText.text = _interactionComponent.GetCurrentInteractable().GetInteractionPrompt();
            
            _fadeTween?.Kill();
            _fadeTween = canvasGroup.DOFade(1f, animationDuration).SetEase(Ease.Flash);
        }

        private void HideUI() {
            _fadeTween = canvasGroup.DOFade(0f, animationDuration).SetEase(Ease.Flash)
                .OnComplete(() => {
                    interactionPromptRoot.SetActive(false);
                });
        }
    }
}