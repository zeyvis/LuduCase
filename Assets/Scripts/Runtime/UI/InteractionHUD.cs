using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LuduCase.Runtime.Core;
using LuduCase.Runtime.Player;

namespace LuduCase.Runtime.UI
{
    /// <summary>
    /// Manages the visual feedback for the interaction system (Prompt text, Progress bar).
    /// </summary>
    public sealed class InteractionHUD : MonoBehaviour
    {
        #region Inspector

        [Header("References")]
        [SerializeField] private InteractionDetector m_detector;
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private TextMeshProUGUI m_promptText;
        [SerializeField] private Image m_progressBar;
        [SerializeField] private GameObject m_progressContainer;

        [Header("Settings")]
        [SerializeField] private float m_fadeSpeed = 10f;

        #endregion

        #region Fields

        private float m_targetAlpha;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_canvasGroup == null)
                m_canvasGroup = GetComponent<CanvasGroup>();

            // Baþlangýçta gizle
            m_targetAlpha = 0f;
            m_canvasGroup.alpha = 0f;

            if (m_progressContainer != null)
                m_progressContainer.SetActive(false);
        }

        private void OnEnable()
        {
            if (m_detector != null)
            {
                m_detector.OnTargetChanged += HandleTargetChanged;
            }
        }

        private void OnDisable()
        {
            if (m_detector != null)
            {
                m_detector.OnTargetChanged -= HandleTargetChanged;
            }
        }

        private void Update()
        {
            UpdateCanvasAlpha();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the fill amount of the hold progress bar.
        /// </summary>
        /// <param name="progress">Normalized progress (0-1).</param>
        public void UpdateProgress(float progress)
        {
            if (m_progressBar == null) return;

            m_progressBar.fillAmount = progress;

            if (m_progressContainer != null)
            {
                bool shouldShow = progress > 0f && progress < 1f;
                if (m_progressContainer.activeSelf != shouldShow)
                {
                    m_progressContainer.SetActive(shouldShow);
                }
            }
        }

        #endregion

        #region Private Methods

        private void HandleTargetChanged(IInteractable target)
        {
            if (target == null)
            {
                m_targetAlpha = 0f;
                UpdateProgress(0f);
            }
            else
            {
                m_targetAlpha = 1f;

                
                var context = new InteractionContext(m_detector.gameObject);
                var data = target.GetPromptData(context);

                
                if (m_promptText != null)
                {
                    
                    if (!string.IsNullOrEmpty(data.CannotInteractReason))
                    {
                        m_promptText.text = $"<color=red>{data.CannotInteractReason}</color>";
                    }
                    else
                    {
                       
                        m_promptText.text = data.PromptText.Replace("{key}", "<color=yellow>E</color>");
                    }
                }
            }
        }

        private void UpdateCanvasAlpha()
        {
            m_canvasGroup.alpha = Mathf.MoveTowards(m_canvasGroup.alpha, m_targetAlpha, Time.deltaTime * m_fadeSpeed);
        }

        #endregion
    }
}