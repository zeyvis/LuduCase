namespace LuduCase.Runtime.Core
{
    /// <summary>
    /// Represents a world object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        bool CanInteract(InteractionContext context);
        InteractionPromptData GetPromptData(InteractionContext context);
        InteractionType InteractionType { get; }
        void Interact(InteractionContext context);
        float HoldDuration { get; }
    }
}
