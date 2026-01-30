using UnityEngine;
using LuduCase.Runtime.Core;

namespace LuduCase.Runtime.Interactables
{
    /// <summary>
    /// Represents a chest that requires a hold interaction to open.
    /// Can spawn an item when opened.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class Chest : MonoBehaviour, IInteractable
    {
        #region Inspector

        [Header("Settings")]
        [Tooltip("How long the player must hold the interaction key (in seconds).")]
        [SerializeField] private float m_holdDuration = 2.0f;

        [Tooltip("The item prefab to spawn when opened (Optional).")]
        [SerializeField] private GameObject m_itemToDrop;

        [Tooltip("Where the item will be spawned.")]
        [SerializeField] private Transform m_dropPoint;

        [Header("Animation")]
        [Tooltip("The lid part of the chest to rotate.")]
        [SerializeField] private Transform m_lidVisual;

        [Tooltip("Rotation angle when open.")]
        [SerializeField] private float m_openAngle = -110f;
        [Header("Audio")]
        [SerializeField] private AudioClip m_openSound;

        #endregion

        #region Fields

        private bool m_isOpened;
        private Quaternion m_closedRotation;
        private Quaternion m_openRotation;
        private AudioSource m_audioSource;

        #endregion

        #region IInteractable Properties


        public InteractionType InteractionType => InteractionType.Hold;


        public float HoldDuration => m_holdDuration;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
            if (m_audioSource == null)
            {
                Debug.LogError("[Chest] AudioSource component is missing. Audio playback will be disabled.", this);
            }

            if (m_lidVisual != null)
            {
                m_closedRotation = m_lidVisual.localRotation;
                m_openRotation = m_closedRotation * Quaternion.Euler(m_openAngle, 0, 0);
            }
            else
            {
                Debug.LogWarning("[Chest] Lid visual is not assigned. Chest will open logically but without animation.", this);
            }

        }

        private void Update()
        {

            if (m_lidVisual != null && m_isOpened)
            {
                m_lidVisual.localRotation = Quaternion.Slerp(
                    m_lidVisual.localRotation,
                    m_openRotation,
                    Time.deltaTime * 5f);
            }
        }

        #endregion

        #region IInteractable Methods

        public bool CanInteract(InteractionContext context)
        {

            return !m_isOpened;
        }

        public InteractionPromptData GetPromptData(InteractionContext context)
        {
            if (m_isOpened)
            {
                return new InteractionPromptData("Empty", false, string.Empty);
            }


            return new InteractionPromptData("Hold {key} to Open", true, string.Empty);
        }

        public void Interact(InteractionContext context)
        {
            if (m_isOpened)
            {
                Debug.LogWarning("[Chest] Interaction ignored. Chest is already opened.", this);
                return;
            }


            m_isOpened = true;
            if (m_openSound != null)
            {
                if (m_audioSource == null)
                {
                    Debug.LogError("[Chest] Cannot play open sound because AudioSource is missing.", this);
                }
                else
                {
                    m_audioSource.PlayOneShot(m_openSound);
                }
            }

            Debug.Log("[Chest] Opened!");


            if (m_itemToDrop != null)
            {
                Vector3 spawnPos;
                if (m_dropPoint != null)
                {
                    spawnPos = m_dropPoint.position;
                }
                else
                {
                    Debug.LogWarning("[Chest] Drop point is not assigned. Using fallback spawn position.", this);
                    spawnPos = transform.position + Vector3.up;
                }

                Instantiate(m_itemToDrop, spawnPos, Quaternion.identity);
            }


        }

        #endregion
    }
}