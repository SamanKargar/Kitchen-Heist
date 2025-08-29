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
        [SerializeField] private RectTransform settingsMenu;
        [SerializeField] private Button startButton;
        [SerializeField] private Button levelsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button backButton;

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

            _canvasGroup.alpha = 0f;
            headerObject.anchoredPosition = new Vector2(headerObject.anchoredPosition.x, headerHiddenYAnchor);
            _startButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _startButtonTransform.anchoredPosition.y);
            _levelsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _levelsButtonTransform.anchoredPosition.y);
            _settingsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _settingsButtonTransform.anchoredPosition.y);
            _quitButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _quitButtonTransform.anchoredPosition.y);
            
            DisableButtons();
        }

        private void OnEnable() {
            startButton.onClick.AddListener(OnStartButtonClick);
            levelsButton.onClick.AddListener(OnLevelsButtonClick);
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
            quitButton.onClick.AddListener(OnQuitButtonClick);
            backButton.onClick.AddListener(OnBackButtonClick);
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
        
        private void AnimateHeader() {
            headerObject.DOAnchorPosY(headerVisibleYAnchor, headerTweenDuration)
                .SetEase(Ease.OutBounce)
                .OnComplete(AnimateButtonsInFromSettings);
        }
        
        private void ShowSettingsMenu() {
            settingsMenu.DOAnchorPosX(250f, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject)
                .OnComplete(() => {
                    UtilsClass.UpdateCursorState(true);
                });
        }

        private void HideSettingsMenu() {
            settingsMenu.DOAnchorPosX(buttonHiddenXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(startButton.gameObject);
        }

        #region - Button State -

        private void DisableButtons() {
            startButton.enabled = false;
            levelsButton.enabled = false;
            settingsButton.enabled = false;
            quitButton.enabled = false;

            _startButtonTrigger.enabled = false;
            _levelsButtonTrigger.enabled = false;
            _settingsButtonTrigger.enabled = false;
            _quitButtonTrigger.enabled = false;
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
        
        private void ResetButtonState(RectTransform rect, GameObject button) {
            if (_moveTweens.TryGetValue(button, out Tween moveTween)) moveTween.Kill();
            if (_scaleTweens.TryGetValue(button, out Tween scaleTween)) scaleTween.Kill();

            rect.localScale = Vector3.one;

            if (_originalAnchoredPos.TryGetValue(button, out Vector2 pos)) {
                rect.anchoredPosition = pos;
            }
        }

        #endregion

        #region - Button Animation -
        
        private void AnimateButtonsInFromSettings() {
            HideSettingsMenu();
            UtilsClass.ExecuteAfterDelay(AnimateStartButton, 0.5f);
        }
        
        private void AnimateButtonsOutToSettings() {
            UtilsClass.UpdateCursorState(false);
            DisableButtons();
            
            ResetButtonState(_quitButtonTransform, quitButton.gameObject);
            ResetButtonState(_settingsButtonTransform, settingsButton.gameObject);
            ResetButtonState(_levelsButtonTransform, levelsButton.gameObject);
            ResetButtonState(_startButtonTransform, startButton.gameObject);

            _quitButtonTransform.DOAnchorPosX(buttonHiddenXAnchor, buttonTweenDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(quitButton.gameObject);

            UtilsClass.ExecuteAfterDelay(() => {
                _settingsButtonTransform.DOAnchorPosX(buttonHiddenXAnchor, buttonTweenDuration)
                    .SetEase(Ease.OutBounce)
                    .SetLink(settingsButton.gameObject);
            }, buttonTweenStartTime);

            UtilsClass.ExecuteAfterDelay(() => {
                _levelsButtonTransform.DOAnchorPosX(buttonHiddenXAnchor, buttonTweenDuration)
                    .SetEase(Ease.OutBounce)
                    .SetLink(levelsButton.gameObject);
            }, buttonTweenStartTime * 2);

            UtilsClass.ExecuteAfterDelay(() => {
                _startButtonTransform.DOAnchorPosX(buttonHiddenXAnchor, buttonTweenDuration)
                    .SetEase(Ease.OutBounce)
                    .SetLink(startButton.gameObject)
                    .OnComplete(ShowSettingsMenu);
            }, buttonTweenStartTime * 3);
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
                .OnComplete(() => {
                    EnableButtons();
                    UtilsClass.EnableUIActionMap();
                    UtilsClass.UpdateCursorState(true);
                });
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
            AnimateButtonsOutToSettings();
        }

        private void OnQuitButtonClick() {
            Application.Quit();
        }

        private void OnBackButtonClick() {
            UtilsClass.UpdateCursorState(false);
            AnimateButtonsInFromSettings();
        }

        #endregion
    }
}