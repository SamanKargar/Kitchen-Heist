using _Game.Scripts.Managers;
using _Game.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class GameStateUI : MonoBehaviour {
        [Header("References")] [Space(6)]
        
        [SerializeField] private GameObject rootObject;
        [SerializeField] private GameObject containerObject;
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private GameObject gameWonButtonList;
        [SerializeField] private GameObject gameLostButtonList;

        [SerializeField] private float containerVisibleYAnchor;
        [SerializeField] private float containerHiddenYAnchor = -800f;
        [SerializeField] private float animationDuration = 0.35f;

        private RectTransform containerRectTransform;
        private Tween _containerTween;

        private void Awake() {
            containerRectTransform = containerObject.GetComponent<RectTransform>();

            containerRectTransform.anchoredPosition = new Vector2(containerRectTransform.anchoredPosition.x, containerHiddenYAnchor);
            rootObject.SetActive(false);
            gameWonButtonList.SetActive(false);
            gameLostButtonList.SetActive(false);
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
            ShowUI(false);
        }

        private void GameEvents_OnGameWonEvent() {
            ShowUI(true);
        }

        private void ShowUI(bool didWin) {
            rootObject.SetActive(true);

            if (didWin) {
                headerText.text = "You Won!";
                gameWonButtonList.SetActive(true);
                gameLostButtonList.SetActive(false);
            }
            else {
                headerText.text = "You Lost!";
                gameLostButtonList.SetActive(true);
                gameWonButtonList.SetActive(false);
            }
            
            UtilsClass.ExecuteAfterDelay(() => {
                _containerTween?.Kill();
                _containerTween = containerRectTransform.DOAnchorPosY(containerVisibleYAnchor, animationDuration)
                    .SetEase(Ease.OutBounce)
                    .SetLink(containerObject); 
            }, 0.25f);
        }

        private void HideUI() {
            _containerTween = containerRectTransform.DOAnchorPosY(containerHiddenYAnchor, animationDuration)
                .SetEase(Ease.InOutBounce)
                .SetLink(containerObject)
                .OnComplete(() => {
                    rootObject.SetActive(false);
                });
        }
    }
}