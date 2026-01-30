using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LuduCase.Runtime.Core;

namespace LuduCase.Runtime.UI
{
    /// <summary>
    /// Manages the visual feedback for interactions (Text & Progress Bar).
    /// </summary>
    public class InteractionHUD : MonoBehaviour
    {
        #region Inspector

        [Header("UI Components")]
        [Tooltip("The text component to display interaction messages.")]
        [SerializeField] private TextMeshProUGUI m_promptText;

        [Tooltip("The container object for the hold progress bar (to hide/show it).")]
        [SerializeField] private GameObject m_progressContainer;

        [Tooltip("The filled image component for the progress bar.")]
        [SerializeField] private Image m_progressFill;

        #endregion

        #region Public Methods


        public void UpdatePrompt(InteractionPromptData data)
        {

            if (m_promptText != null)
            {
                m_promptText.text = data.PromptText;

                m_promptText.gameObject.SetActive(!string.IsNullOrEmpty(data.PromptText));
            }


            if (m_progressContainer != null)
            {
                m_progressContainer.SetActive(data.ShowHoldProgress);
            }
        }


        public void UpdateProgress(float percentage)
        {
            if (m_progressFill != null)
            {
                m_progressFill.fillAmount = percentage;
            }
        }


        public void Hide()
        {
            if (m_promptText != null) m_promptText.gameObject.SetActive(false);
            if (m_progressContainer != null) m_progressContainer.SetActive(false);
        }

        #endregion
    }
}