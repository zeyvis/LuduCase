using UnityEngine;
using LuduCase.Runtime.Core;
using LuduCase.Runtime.UI;

namespace LuduCase.Runtime.Player
{
    /// <summary>
    /// Handles input processing for interactions (Instant, Hold, Toggle).
    /// </summary>
    [RequireComponent(typeof(InteractionDetector))]
    public sealed class InteractionInputHandler : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private InteractionDetector m_detector;
        [SerializeField] private InteractionHUD m_hud;
        [SerializeField] private KeyCode m_interactionKey = KeyCode.E;

        #endregion

        #region Fields

        private float m_holdTimer;
        private bool m_isHolding;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (m_detector == null)
            {
                m_detector = GetComponent<InteractionDetector>();
                if (m_detector == null)
                {
                    Debug.LogError("[InteractionInputHandler] InteractionDetector component is missing. Interactions will not work.", this);
                }
            }

        }

        private void Update()
        {
            HandleInput();
        }

        #endregion

        #region Private Methods

       private void HandleInput()
        {
            if (m_detector == null)
            {
                Debug.LogError("[InteractionInputHandler] Detector reference is null. Input handling aborted.", this);
                return;
            }

            IInteractable currentTarget = m_detector.GetCurrentInteractable();


            if (currentTarget == null)
            {
                ResetHold();
                if (m_hud == null)
                {
                    Debug.LogError("[InteractionInputHandler] HUD reference is null. Prompt cannot be updated.", this);
                }
                else
                {
                    m_hud.UpdatePrompt(new InteractionPromptData("", false, ""));
                }
                return;
            }

            var context = new InteractionContext(gameObject);


            var data = currentTarget.GetPromptData(context);
            string cleanText = data.PromptText.Replace("{key}", m_interactionKey.ToString());
            data = new InteractionPromptData(cleanText, data.ShowHoldProgress, data.CannotInteractReason);

            if (m_hud == null)
            {
                Debug.LogError("[InteractionInputHandler] HUD reference is null. Prompt cannot be updated.", this);
            }
            else
            {
                m_hud.UpdatePrompt(data);
            }



            if (!currentTarget.CanInteract(context))
            {
                ResetHold();
                return;
            }

            switch (currentTarget.InteractionType)
            {
                case InteractionType.Instant:
                case InteractionType.Toggle:
                    HandleInstantInteraction(currentTarget, context);
                    break;

                case InteractionType.Hold:
                    HandleHoldInteraction(currentTarget, context);
                    break;
            }
        }

        private void HandleInstantInteraction(IInteractable target, InteractionContext context)
        {
            if (Input.GetKeyDown(m_interactionKey))
            {
                target.Interact(context);
            }
        }

        private void HandleHoldInteraction(IInteractable target, InteractionContext context)
        {
            if (Input.GetKey(m_interactionKey))
            {
                m_isHolding = true;
                m_holdTimer += Time.deltaTime;

                float duration = target.HoldDuration;
                if (duration <= 0f)
                {
                    Debug.LogError("[InteractionInputHandler] HoldDuration is zero or negative. Interaction aborted.", this);
                    ResetHold();
                    return;
                }

                float progress = Mathf.Clamp01(m_holdTimer / duration);


                if (m_hud != null)
                {
                    m_hud.UpdateProgress(progress);
                }

                if (m_holdTimer >= duration)
                {
                    target.Interact(context);
                    ResetHold();
                }
            }
            else
            {
                ResetHold();
            }
        }

        private void ResetHold()
        {
            m_isHolding = false;
            m_holdTimer = 0f;

            if (m_hud != null)
            {
                m_hud.UpdateProgress(0f);
            }
        }

        #endregion
    }
}