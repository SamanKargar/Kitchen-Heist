using System;
using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        [SerializeField] private float containerVisibleYAnchor;
        [SerializeField] private float containerHiddenYAnchor = -800f;
        [SerializeField] private float animationDuration = 0.35f;

        private RectTransform containerRectTransform;
        private Tween _containerTween;

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
            
            UtilsClass.ExecuteAfterDelay(() => {
                _containerTween?.Kill();
                _containerTween = containerRectTransform.DOAnchorPosY(containerVisibleYAnchor, animationDuration)
                    .SetEase(Ease.OutBounce)
                    .SetLink(containerObject); 
            }, 0.25f);
        }

        private void HideUI(Action onComplete) {
            _containerTween = containerRectTransform.DOAnchorPosY(containerHiddenYAnchor, animationDuration)
                .SetEase(Ease.OutBounce)
                .SetLink(containerObject)
                .OnComplete(() => {
                    onComplete.Invoke();
                    rootObject.SetActive(false);
                });
        }

        private void OnClickRestartButton() {
            HideUI(() => SceneManager.LoadScene(0));
        }

        private void OnClickNextLevelButton() {
            HideUI(() => {
                Debug.Log("Go Next Level");
            });
        }

        private void OnClickMainMenuButton() {
            HideUI(() => {
                Debug.Log("Go Main Menu");
            });
        }

        private void OnClickQuitButton() {
            HideUI(Application.Quit);
        }
    }
}