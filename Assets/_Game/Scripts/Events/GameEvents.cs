using System;

namespace _Game.Scripts.Events {
    public class GameEvents {
        public event Action OnGameWonEvent;
        public event Action OnGameLostEvent;

        public void OnGameWon() {
            OnGameWonEvent?.Invoke();
        }

        public void OnGameLost() {
            OnGameLostEvent?.Invoke();
        }
    }
}