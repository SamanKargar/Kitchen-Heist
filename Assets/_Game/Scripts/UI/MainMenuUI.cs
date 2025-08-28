using System.Collections.Generic;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class MainMenuUI : MonoBehaviour {
        [Header("References")] [Space(6)]
        [SerializeField] private GameObject rootObject;
        [SerializeField] private RectTransform headerObject;
        [SerializeField] private Button startButton;
        [SerializeField] private Button levelsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [Space(6)] [Header("Animation")] [Space(6)]
        [SerializeField] private float headerVisibleYAnchor = -130f;
        [SerializeField] private float headerHiddenYAnchor = 115f;
        [SerializeField] private float buttonVisibleXAnchor = 250f;
        [SerializeField] private float buttonHiddenXAnchor = -265f;
        [SerializeField] private float headerTweenDuration = 0.5f;
        [SerializeField] private float fadeTweenDuration = 0.75f;
        [SerializeField] private float buttonTweenDuration = 0.75f;
        [SerializeField] private float buttonTweenStartTime = 0.35f;
        [SerializeField] private float buttonScaleTweenDuration = 0.35f;
        [SerializeField] private float buttonHorizontalTweenAmount = 5f;

        private CanvasGroup _canvasGroup;
        
        private RectTransform _startButtonTransform;
        private RectTransform _levelsButtonTransform;
        private RectTransform _settingsButtonTransform;
        private RectTransform _quitButtonTransform;
        
        private EventTrigger _startButtonTrigger;
        private EventTrigger _levelsButtonTrigger;
        private EventTrigger _settingsButtonTrigger;
        private EventTrigger _quitButtonTrigger;

        private readonly Dictionary<GameObject, Tween> _moveTweens = new Dictionary<GameObject, Tween>();
        private readonly Dictionary<GameObject, Tween> _scaleTweens = new Dictionary<GameObject, Tween>();
        private readonly Dictionary<GameObject, Vector2> _originalAnchoredPos = new Dictionary<GameObject, Vector2>();

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _startButtonTransform = startButton.GetComponent<RectTransform>();
            _levelsButtonTransform = levelsButton.GetComponent<RectTransform>();
            _settingsButtonTransform = settingsButton.GetComponent<RectTransform>();
            _quitButtonTransform = quitButton.GetComponent<RectTransform>();

            _startButtonTrigger = startButton.GetComponent<EventTrigger>();
            _levelsButtonTrigger = levelsButton.GetComponent<EventTrigger>();
            _settingsButtonTrigger = settingsButton.GetComponent<EventTrigger>();
            _quitButtonTrigger = quitButton.GetComponent<EventTrigger>();

            startButton.enabled = false;
            levelsButton.enabled = false;
            settingsButton.enabled = false;
            quitButton.enabled = false;

            _startButtonTrigger.enabled = false;
            _levelsButtonTrigger.enabled = false;
            _settingsButtonTrigger.enabled = false;
            _quitButtonTrigger.enabled = false;

            _canvasGroup.alpha = 0f;
            headerObject.anchoredPosition = new Vector2(headerObject.anchoredPosition.x, headerHiddenYAnchor);
            _startButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _startButtonTransform.anchoredPosition.y);
            _levelsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _levelsButtonTransform.anchoredPosition.y);
            _settingsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _settingsButtonTransform.anchoredPosition.y);
            _quitButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _quitButtonTransform.anchoredPosition.y);
        }

        private void OnEnable() {
            startButton.onClick.AddListener(OnStartButtonClick);
            levelsButton.onClick.AddListener(OnLevelsButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
        }

        private void OnDisable() {
            startButton.onClick.RemoveAllListeners();
            levelsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }

        private void Start() {
            _canvasGroup.DOFade(1f, fadeTweenDuration)
                .SetEase(Ease.Flash)
                .OnComplete(AnimateHeader);
        }
        
        private void EnableButtons() {
            startButton.enabled = true;
            levelsButton.enabled = true;
            settingsButton.enabled = true;
            quitButton.enabled = true;
            
            _startButtonTrigger.enabled = true;
            _levelsButtonTrigger.enabled = true;
            _settingsButtonTrigger.enabled = true;
            _quitButtonTrigger.enabled = true;
        }

        #region - Button Animation -

        private void AnimateHeader() {
            headerObject.DOAnchorPosY(headerVisibleYAnchor, headerTweenDuration)
                .SetEase(Ease.OutBounce)
                .OnComplete(AnimateStartButton);
        }

        private void AnimateStartButton() {
            _startButtonTransform.DOAnchorPosX(buttonVisibleXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject);
            
            UtilsClass.ExecuteAfterDelay(AnimateLevelsButton, buttonTweenStartTime);
        }

        private void AnimateLevelsButton() {
            _levelsButtonTransform.DOAnchorPosX(buttonVisibleXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject);
            
            UtilsClass.ExecuteAfterDelay(AnimateSettingsButton, buttonTweenStartTime);
        }

        private void AnimateSettingsButton() {
            _settingsButtonTransform.DOAnchorPosX(buttonVisibleXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject);
            
            UtilsClass.ExecuteAfterDelay(AnimateQuitButton, buttonTweenStartTime);
        }

        private void AnimateQuitButton() {
            _quitButtonTransform.DOAnchorPosX(buttonVisibleXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject)
                .OnComplete(EnableButtons);
        }

        #endregion

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
                .SetLink(button);

            _scaleTweens[button] = rect.DOScale(
                    Vector3.one * 1.125f,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetLink(button);
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
                .SetLink(button);

            _scaleTweens[button] = rect.DOScale(
                    Vector3.one,
                    buttonScaleTweenDuration)
                .SetEase(Ease.OutQuad)
                .SetLink(button);
        }

        #endregion

        #region - Button Click -

        private void OnStartButtonClick() {
            Loader.Load(Loader.Scene.PrototypingScene);
        }

        private void OnLevelsButtonClick() {
            Debug.Log("On Levels Button Clicked");
        }

        private void OnSettingsButtonClick() {
            Debug.Log("On Settings Button Clicked");
        }

        private void OnQuitButtonClick() {
            Application.Quit();
        }

        #endregion
    }
}