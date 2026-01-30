using System;
using UnityEngine;
using LuduCase.Runtime.Core;

namespace LuduCase.Runtime.Player
{
    /// <summary>
    /// Raycast-based detector that finds the nearest IInteractable in front of the camera.
    /// </summary>
    public sealed class InteractionDetector : MonoBehaviour
    {
        #region Inspector

        [SerializeField] private Transform m_raycastOrigin;
        [SerializeField] private float m_maxDistance = 2.5f;
        [SerializeField] private float m_sphereRadius = 0.15f;
        [SerializeField] private LayerMask m_layerMask = ~0;
        [SerializeField] private bool m_debugLogs = true;

        #endregion

        #region Events

        public event Action<IInteractable> OnTargetChanged;

        #endregion

        #region Fields

        private IInteractable m_current;

        #endregion

        #region Unity

        private void Awake()
        {
            if (m_raycastOrigin == null)
            {
                if (Camera.main != null)
                {
                    m_raycastOrigin = Camera.main.transform;
                }
                else
                {
                    Debug.LogError("[InteractionDetector] Raycast origin is not assigned and no Main Camera found. Detection will not work.", this);
                }
            }

        }

        private void Update()
        {
            Detect();
        }

        #endregion

        #region Public

        public IInteractable GetCurrentInteractable()
        {
            return m_current;
        }

        #endregion

        #region Detection

        private void Detect()
        {
            if (m_raycastOrigin == null)
            {
                Debug.LogError("[InteractionDetector] Raycast origin is null. Detection skipped.", this);
                SetTarget(null);
                return;
            }


            Ray ray = new Ray(m_raycastOrigin.position, m_raycastOrigin.forward);

            RaycastHit[] hits = Physics.SphereCastAll(
                ray,
                m_sphereRadius,
                m_maxDistance,
                m_layerMask,
                QueryTriggerInteraction.Collide);

            IInteractable nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (var hit in hits)
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable == null)
                {
                    if (m_debugLogs)
                        Debug.LogWarning("[InteractionDetector] Hit object has no IInteractable component.", hit.collider.gameObject);
                    continue;
                }


                if (hit.distance < nearestDistance)
                {
                    nearest = interactable;
                    nearestDistance = hit.distance;
                }
            }

            SetTarget(nearest);
        }

        private void SetTarget(IInteractable newTarget)
        {
            if (ReferenceEquals(m_current, newTarget))
                return;

            m_current = newTarget;

            if (m_debugLogs)
            {
                string name = "null";
                if (m_current is Component c)
                    name = c.gameObject.name;

                Debug.Log($"[InteractionDetector] Interactable: {name}", this);
            }

            if (OnTargetChanged == null)
            {
                Debug.LogWarning("[InteractionDetector] OnTargetChanged event has no listeners.", this);
            }
            else
            {
                OnTargetChanged.Invoke(m_current);
            }

        }

        #endregion
    }
}
