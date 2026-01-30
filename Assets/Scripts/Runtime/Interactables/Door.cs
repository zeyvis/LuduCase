using UnityEngine;
using LuduCase.Runtime.Core;
using LuduCase.Runtime.Player;
using LuduCase.Runtime.ScriptableObjects;

namespace LuduCase.Runtime.Interactables
{
    /// <summary>
    /// Represents a door that can be locked, unlocked, opened, and closed.
    /// </summary>
    public sealed class Door : MonoBehaviour, IInteractable
    {
        #region Inspector

        [Header("Settings")]
        [Tooltip("If true, the door starts locked and requires a specific key.")]
        [SerializeField] private bool m_isLocked;

        [Tooltip("The key required to unlock this door.")]
        [SerializeField] private KeyData m_requiredKey;

        [Header("Animation")]
        [Tooltip("The visual part of the door to rotate.")]
        [SerializeField] private Transform m_doorVisual;

        [Tooltip("Rotation angle when open.")]
        [SerializeField] private float m_openAngle = 90f;

        [Tooltip("Speed of opening/closing animation.")]
        [SerializeField] private float m_animationSpeed = 2f;

        #endregion

        #region Fields

        private bool m_isOpen;
        private Quaternion m_closedRotation;
        private Quaternion m_openRotation;

        #endregion

        #region IInteractable Properties

        public InteractionType InteractionType => InteractionType.Toggle;

        public float HoldDuration => 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_doorVisual != null)
            {
                m_closedRotation = m_doorVisual.localRotation;
                m_openRotation = m_closedRotation * Quaternion.Euler(0, m_openAngle, 0);
            }
            else
            {
                m_doorVisual = transform;
                m_closedRotation = transform.localRotation;
                m_openRotation = m_closedRotation * Quaternion.Euler(0, m_openAngle, 0);
            }
        }

        private void Update()
        {
            if (m_doorVisual != null)
            {
                Quaternion targetRot = m_isOpen ? m_openRotation : m_closedRotation;
                m_doorVisual.localRotation = Quaternion.Slerp(
                    m_doorVisual.localRotation,
                    targetRot,
                    Time.deltaTime * m_animationSpeed);
            }
        }

        #endregion

        #region IInteractable Methods

        public bool CanInteract(InteractionContext context)
        {
            return true;
        }

        public InteractionPromptData GetPromptData(InteractionContext context)
        {
            if (m_isLocked)
            {
                var inventory = context.Interactor.GetComponent<Inventory>();
                bool hasKey = inventory != null && m_requiredKey != null && inventory.HasKey(m_requiredKey);

                if (hasKey)
                {
                    return new InteractionPromptData($"Unlock with {m_requiredKey.KeyName}", false, string.Empty);
                }
                else
                {
                    string reason = m_requiredKey != null ? $"Locked ({m_requiredKey.KeyName} required)" : "Locked";
                    return new InteractionPromptData("Locked", false, reason);
                }
            }

            string prompt = m_isOpen ? "Close" : "Open";
            return new InteractionPromptData(prompt, false, string.Empty);
        }

        public void Interact(InteractionContext context)
        {
            if (m_isLocked)
            {
                var inventory = context.Interactor.GetComponent<Inventory>();
                if (inventory != null && m_requiredKey != null && inventory.HasKey(m_requiredKey))
                {
                    m_isLocked = false; 
                    Debug.Log("[Door] Unlocked!");
                }
                else
                {
                    return;
                }
            }
            else
            {
                m_isOpen = !m_isOpen;
            }
        }

        #endregion
    }
}