using UnityEngine;
using UnityEngine.Events;
using LuduCase.Runtime.Core;

namespace LuduCase.Runtime.Interactables
{
    /// <summary>
    /// A switch that triggers UnityEvents when toggled.
    /// Demonstrates the observer pattern/event-driven architecture.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class Switch : MonoBehaviour, IInteractable
    {
        #region Inspector

        [Header("Settings")]
        [SerializeField] private bool m_isOn;

        [Header("Events")]
        [Tooltip("Triggered when the switch state changes. True = On, False = Off.")]
        [SerializeField] private UnityEvent<bool> m_onToggle;

        [Header("Animation")]
        [SerializeField] private Transform m_handleVisual;
        [SerializeField] private float m_handleAngle = 45f;
        [Header("Audio")]
        [SerializeField] private AudioClip m_switchSound;

        #endregion

        #region Fields
        private AudioSource m_audioSource;
        #endregion

        #region IInteractable Properties

        public InteractionType InteractionType => InteractionType.Toggle;
        public float HoldDuration => 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            UpdateVisual();
        }

        #endregion

        #region IInteractable Methods

        public bool CanInteract(InteractionContext context)
        {
            return true;
        }

        public InteractionPromptData GetPromptData(InteractionContext context)
        {
            string status = m_isOn ? "Turn Off" : "Turn On";
            return new InteractionPromptData(status, false, string.Empty);
        }

        public void Interact(InteractionContext context)
        {
            m_isOn = !m_isOn;

            m_onToggle?.Invoke(m_isOn);
            if (m_switchSound == null)
            {
                Debug.LogWarning("[Switch] Switch sound is not assigned. SFX will be skipped.", this);
            }
            else if (m_audioSource == null)
            {
                Debug.LogError("[Switch] Cannot play SFX because AudioSource is missing.", this);
            }
            else
            {
                m_audioSource.PlayOneShot(m_switchSound);
            }


            UpdateVisual();
            Debug.Log($"[Switch] Toggled: {m_isOn}");
        }

        #endregion

        #region Private Methods

        private void UpdateVisual()
        {
            if (m_handleVisual == null)
            {
                Debug.LogWarning("[Switch] Handle visual is not assigned. Visual update skipped.", this);
                return;
            }

            float angle = m_isOn ? m_handleAngle : -m_handleAngle;
            m_handleVisual.localRotation = Quaternion.Euler(0, 0, angle);

        }

        #endregion
    }
}