using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;
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

        private CanvasGroup _canvasGroup;
        private RectTransform _startButtonTransform;
        private RectTransform _levelsButtonTransform;
        private RectTransform _settingsButtonTransform;
        private RectTransform _quitButtonTransform;

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _startButtonTransform = startButton.GetComponent<RectTransform>();
            _levelsButtonTransform = levelsButton.GetComponent<RectTransform>();
            _settingsButtonTransform = settingsButton.GetComponent<RectTransform>();
            _quitButtonTransform = quitButton.GetComponent<RectTransform>();

            startButton.enabled = false;
            levelsButton.enabled = false;
            settingsButton.enabled = false;
            quitButton.enabled = false;

            _canvasGroup.alpha = 0f;
            headerObject.anchoredPosition = new Vector2(headerObject.anchoredPosition.x, headerHiddenYAnchor);
            _startButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _startButtonTransform.anchoredPosition.y);
            _levelsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _levelsButtonTransform.anchoredPosition.y);
            _settingsButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _settingsButtonTransform.anchoredPosition.y);
            _quitButtonTransform.anchoredPosition = new Vector2(buttonHiddenXAnchor, _quitButtonTransform.anchoredPosition.y);
        }

        private void Start() {
            _canvasGroup.DOFade(1f, fadeTweenDuration)
                .SetEase(Ease.Flash)
                .OnComplete(AnimateHeader);
        }

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

        private void EnableButtons() {
            startButton.enabled = true;
            levelsButton.enabled = true;
            settingsButton.enabled = true;
            quitButton.enabled = true;
        }
    }
}