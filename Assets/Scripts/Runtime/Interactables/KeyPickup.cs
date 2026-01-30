using UnityEngine;
using LuduCase.Runtime.Core;
using LuduCase.Runtime.Player;
using LuduCase.Runtime.ScriptableObjects;

namespace LuduCase.Runtime.Interactables
{
    /// <summary>
    /// An interactable item that adds a key to the player's inventory when picked up.
    /// </summary>
    public sealed class KeyPickup : MonoBehaviour, IInteractable
    {
        #region Inspector

        [SerializeField] private KeyData m_keyData;
        [SerializeField] private string m_prompt = "Pick up {key}";

        #endregion

        #region IInteractable Properties

        public InteractionType InteractionType => InteractionType.Instant;
        public float HoldDuration => 0f;

        #endregion

        #region IInteractable Methods

        public bool CanInteract(InteractionContext context)
        {
            return m_keyData != null;
        }

        public InteractionPromptData GetPromptData(InteractionContext context)
        {
            string promptText = m_prompt;
            if (m_keyData != null)
            {
                promptText += $" {m_keyData.KeyName}";
            }

            return new InteractionPromptData(promptText, false, string.Empty);
        }

        public void Interact(InteractionContext context)
        {
            var inventory = context.Interactor.GetComponent<Inventory>();

            if (inventory != null && m_keyData != null)
            {
                inventory.AddKey(m_keyData);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("[KeyPickup] Inventory component not found on interactor!", this);
            }
        }

        #endregion
    }
}