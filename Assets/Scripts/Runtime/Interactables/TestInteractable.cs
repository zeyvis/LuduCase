using UnityEngine;
using LuduCase.Runtime.Core;

namespace LuduCase.Runtime.Interactables
{
    /// <summary>
    /// Simple interactable used to validate detection + UI + input pipeline.
    /// </summary>
    public sealed class TestInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string m_prompt = "Press {key} to Test";

        public InteractionType InteractionType => InteractionType.Instant;
        public float HoldDuration => 0f;

        public bool CanInteract(InteractionContext context)
        {
            return true;
        }

        public InteractionPromptData GetPromptData(InteractionContext context)
        {
            return new InteractionPromptData(m_prompt, false, string.Empty);
        }

        public void Interact(InteractionContext context)
        {
            Debug.Log("[TestInteractable] Interact() called", this);
        }
    }
}
