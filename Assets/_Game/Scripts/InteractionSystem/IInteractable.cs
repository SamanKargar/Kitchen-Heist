namespace _Game.Scripts.InteractionSystem {
    public interface IInteractable {
        string GetInteractionPrompt();
        void Interact();
    }
}