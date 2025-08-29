using System;
using System.Collections.Generic;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class PauseMenuUI : MonoBehaviour {
        [SerializeField] private GameObject rootObject;
        [SerializeField] private GameObject containerObject;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private float containerVisibleYAnchor;
        [SerializeField] private float containerHiddenYAnchor = -800f;
        [SerializeField] private float animationDuration = 0.35f;
        [SerializeField] private float buttonScaleTweenDuration = 0.35f;
        [SerializeField] private float buttonHorizontalTweenAmount = 5f;

        private Tween _containerTween;
        
        private RectTransform containerRectTransform;
        private EventTrigger _resumeButtonTrigger;
        private EventTrigger _settingsButtonTrigger;
        private EventTrigger _mainMenuButtonTrigger;
        private EventTrigger _quitButtonTrigger;

        private readonly Dictionary<GameObject, Tween> _moveTweens = new Dictionary<GameObject, Tween>();
        private readonly Dictionary<GameObject, Tween> _scaleTweens = new Dictionary<GameObject, Tween>();
        private readonly Dictionary<GameObject, Vector2> _originalAnchoredPos = new Dictionary<GameObject, Vector2>();

        private void Awake() {
            containerRectTransform = containerObject.GetComponent<RectTransform>();
            _resumeButtonTrigger = resumeButton.GetComponent<EventTrigger>();
            _settingsButtonTrigger = settingsButton.GetComponent<EventTrigger>();
            _mainMenuButtonTrigger = mainMenuButton.GetComponent<EventTrigger>();
            _quitButtonTrigger = quitButton.GetComponent<EventTrigger>();
            
            containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, containerHiddenYAnchor);
            rootObject.SetActive(false);

            resumeButton.onClick.AddListener(OnResumeButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnPauseActionEvent += InputEvents_OnPauseActionEvent;
            GameEventsManager.Instance.InputEvents.OnCancelActionEvent += InputEvents_OnCancelActionEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnPauseActionEvent -= InputEvents_OnPauseActionEvent;
            GameEventsManager.Instance.InputEvents.OnCancelActionEvent -= InputEvents_OnCancelActionEvent;
        }

        private void InputEvents_OnCancelActionEvent() {
            HideUI(() => {
                UtilsClass.EnableGameplayActionMap();
                UtilsClass.UpdateCursorState(false);
            });
        }

        private void InputEvents_OnPauseActionEvent() {
            UtilsClass.EnableUIActionMap();
            UtilsClass.UpdateCursorState(true);
            
            ShowUI();
        }

        private void ShowUI() {
            DisableButtons();
            
            Time.timeScale = 0f;
            rootObject.SetActive(true);
            _containerTween?.Kill();
            _containerTween = containerRectTransform.DOAnchorPosY(containerVisibleYAnchor, animationDuration)
                .SetEase(Ease.OutBounce)
                .SetUpdate(true)
                .SetLink(containerObject)
                .OnComplete(EnableButtons);
        }

        private void HideUI(Action onComplete) {
            DisableButtons();
            
            _containerTween = containerRectTransform.DOAnchorPosY(containerHiddenYAnchor, animationDuration)
                .SetEase(Ease.OutBounce)
                .SetUpdate(true)
                .SetLink(containerObject)
                .OnComplete(() => {
                    Time.timeScale = 1f;
                    onComplete();
                });
        }
        
        private void DisableButtons() {
            resumeButton.enabled = false;
            settingsButton.enabled = false;
            mainMenuButton.enabled = false;
            quitButton.enabled = false;

            _resumeButtonTrigger.enabled = false;
            _settingsButtonTrigger.enabled = false;
            _mainMenuButtonTrigger.enabled = false;
            _quitButtonTrigger.enabled = false;
        }
        
        private void EnableButtons() {
            resumeButton.enabled = true;
            settingsButton.enabled = true;
            mainMenuButton.enabled = true;
            quitButton.enabled = true;

            _resumeButtonTrigger.enabled = true;
            _settingsButtonTrigger.enabled = true;
            _mainMenuButtonTrigger.enabled = true;
            _quitButtonTrigger.enabled = true;
        }
        
        #region - Button Hover -

        private void CacheOriginalPos(GameObject button, RectTransform rect) {
            if (!_originalAnchoredPos.ContainsKey(button)) {
                _originalAnchoredPos[button] = rect.anchoredPosition;
            }
        }

        public void OnHoverEnter(GameObject button) {
            RectTransform rect = button.GetComponent<RectTransform>();
            CacheOriginalPos(button, rect);

            if (_moveTweens.TryGetValue(button, out Tween moveTween)) moveTween.Kill();
            if (_scaleTweens.TryGetValue(button, out Tween scaleTween)) scaleTween.Kill();

            _moveTweens[button] = rect.DOAnchorPosX(
                    _originalAnchoredPos[button].x + buttonHorizontalTweenAmount,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(button);

            _scaleTweens[button] = rect.DOScale(
                    Vector3.one * 1.125f,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(button);
            
            GameEventsManager.Instance.UIEvents.OnButtonHoverEnter();
        }

        public void OnHoverExit(GameObject button) {
            RectTransform rect = button.GetComponent<RectTransform>();
            CacheOriginalPos(button, rect);

            if (_moveTweens.TryGetValue(button, out Tween moveTween)) moveTween.Kill();
            if (_scaleTweens.TryGetValue(button, out Tween scaleTween)) scaleTween.Kill();

            _moveTweens[button] = rect.DOAnchorPosX(
                    _originalAnchoredPos[button].x,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(button);

            _scaleTweens[button] = rect.DOScale(
                    Vector3.one,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true)
                .SetLink(button);
        }

        #endregion

        #region - Button Click -

        private void OnResumeButtonClick() {
            GameEventsManager.Instance.UIEvents.OnButtonClick();
            HideUI(() => {
                rootObject.SetActive(false);
                UtilsClass.EnableGameplayActionMap();
                UtilsClass.UpdateCursorState(false);
            });
        }

        private void OnSettingsButtonClick() {
            GameEventsManager.Instance.UIEvents.OnButtonClick();
            HideUI(() => {
                Debug.Log("Open Settings Menu.");
            });
        }

        private void OnMainMenuButtonClick() {
            GameEventsManager.Instance.UIEvents.OnButtonClick();
            HideUI(() => {
                Loader.Load(Loader.Scene.MainMenu);
            });
        }

        private void OnQuitButtonClick() {
            GameEventsManager.Instance.UIEvents.OnButtonClick();
            HideUI(Application.Quit);
        }

        #endregion
    }
}