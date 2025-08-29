using System;
using System.Collections.Generic;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class GameStateUI : MonoBehaviour {
        [Header("References")] [Space(6)]
        
        [SerializeField] private GameObject rootObject;
        [SerializeField] private GameObject containerObject;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;

        [Space(6)] [Header("Animation")] [Space(6)]
        
        [SerializeField] private float containerVisibleYAnchor;
        [SerializeField] private float containerHiddenYAnchor = -800f;
        [SerializeField] private float animationDuration = 0.35f;
        [SerializeField] private float buttonScaleTweenDuration = 0.35f;
        [SerializeField] private float buttonHorizontalTweenAmount = 5f;

        private Tween _containerTween;
        private RectTransform containerRectTransform;
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
            containerRectTransform = containerObject.GetComponent<RectTransform>();

            containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, containerHiddenYAnchor);
            rootObject.SetActive(false);
            
            restartButton.onClick.AddListener(OnClickRestartButton);
            nextLevelButton.onClick.AddListener(OnClickNextLevelButton);
            mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
            quitButton.onClick.AddListener(OnClickQuitButton);
        }

        private void OnEnable() {
            GameEventsManager.Instance.GameEvents.OnGameWonEvent += GameEvents_OnGameWonEvent;
            GameEventsManager.Instance.GameEvents.OnGameLostEvent += GameEvents_OnGameLostEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.GameEvents.OnGameWonEvent -= GameEvents_OnGameWonEvent;
            GameEventsManager.Instance.GameEvents.OnGameLostEvent -= GameEvents_OnGameLostEvent;
        }

        private void GameEvents_OnGameLostEvent() {
            UtilsClass.EnableUIActionMap();
            UtilsClass.UpdateCursorState(true);
            
            ShowUI(false);
        }

        private void GameEvents_OnGameWonEvent() {
            UtilsClass.EnableUIActionMap();
            UtilsClass.UpdateCursorState(true);
            
            ShowUI(true);
        }

        private void ShowUI(bool didWin) {
            Time.timeScale = 0f;
            rootObject.SetActive(true);

            if (didWin) {
                headerText.text = "You Won!";
                nextLevelButton.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(false);
            }
            else {
                headerText.text = "You Lost!";
                nextLevelButton.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(true);
            }
            
            _containerTween?.Kill();
            _containerTween = containerRectTransform.DOAnchorPosY(containerVisibleYAnchor, animationDuration)
                .SetEase(Ease.OutBounce)
                .SetUpdate(true)
                .SetLink(containerObject);
        }

        private void HideUI(Action onComplete) {
            _containerTween = containerRectTransform.DOAnchorPosY(containerHiddenYAnchor, animationDuration)
                .SetEase(Ease.OutBounce)
                .SetUpdate(true)
                .SetLink(containerObject)
                .OnComplete(onComplete.Invoke);
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

        #region - Button Clicks -

        private void OnClickRestartButton() {
            HideUI(() => Loader.Load(Loader.Scene.PrototypingScene));
        }

        private void OnClickNextLevelButton() {
            HideUI(() => {
                Debug.Log("Go Next Level");
            });
        }

        private void OnClickMainMenuButton() {
            HideUI(() => Loader.Load(Loader.Scene.MainMenu));
        }

        private void OnClickQuitButton() {
            HideUI(Application.Quit);
        }

        #endregion
    }
}