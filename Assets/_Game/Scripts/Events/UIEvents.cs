using System;

namespace _Game.Scripts.Events {
    public class UIEvents {
        public event Action OnButtonHoverEnterEvent;
        public event Action OnButtonHoverExitEvent;

        public event Action OnButtonClickEvent;

        public event Action OnSliderValueChangeEvent;

        public event Action OnButtonAnimateEvent;

        public void OnButtonHoverEnter() {
            OnButtonHoverEnterEvent?.Invoke();
        }

        public void OnButtonHoverExit() {
            OnButtonHoverExitEvent?.Invoke();
        }

        public void OnButtonClick() {
            OnButtonClickEvent?.Invoke();
        }

        public void OnSliderValueChange() {
            OnSliderValueChangeEvent?.Invoke();
        }

        public void OnButtonAnimate() {
            OnButtonAnimateEvent?.Invoke();
        }
    }
}