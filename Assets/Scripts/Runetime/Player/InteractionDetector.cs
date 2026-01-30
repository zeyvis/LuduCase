using System;
using UnityEngine;

namespace LuduCase.Runtime.Player
{
    /// <summary>
    /// Raycast-based detector that finds the best interactable in front of the camera.
    /// Keeps a single active target and notifies listeners when it changes.
    /// </summary>
    public sealed class InteractionDetector : MonoBehaviour
    {
        #region Inspector

        [Header("Raycast")]
        [SerializeField] private Transform m_raycastOrigin;
        [SerializeField] private float m_maxDistance = 2.25f;
        [SerializeField] private LayerMask m_interactableMask = ~0;

        [Header("Selection")]
        [SerializeField] private float m_sphereRadius = 0.15f;

        [Header("Debug")]
        [SerializeField] private bool m_drawDebug = true;

        #endregion

        #region Public Events

        /// <summary>
        /// Fired whenever the current detected interactable changes.
        /// </summary>
        public event Action<GameObject> OnTargetChanged;

        #endregion

        #region Private Fields

        private GameObject m_currentTarget;

        #endregion

        #region Unity Methods

        private void Reset()
        {
            if (m_raycastOrigin == null && Camera.main != null)
            {
                m_raycastOrigin = Camera.main.transform;
            }
        }

        private void Awake()
        {
            if (m_raycastOrigin == null)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    m_raycastOrigin = mainCam.transform;
                }
                else
                {
                    Debug.LogError($"{nameof(InteractionDetector)} requires a raycast origin. Assign a camera transform.", this);
                }
            }
        }

        private void Update()
        {
            Detect();
        }

        #endregion

        #region Public API

        /// <summary>
        /// Returns the current detected target (may be null).
        /// </summary>
        public GameObject GetCurrentTarget()
        {
            return m_currentTarget;
        }

        #endregion

        #region Internal

        private void Detect()
        {
            if (m_raycastOrigin == null)
            {
                SetTarget(null);
                return;
            }

            Ray ray = new Ray(m_raycastOrigin.position, m_raycastOrigin.forward);

            bool hasHit = Physics.SphereCast(
                ray,
                m_sphereRadius,
                out RaycastHit hit,
                m_maxDistance,
                m_interactableMask,
                QueryTriggerInteraction.Collide);

            if (!hasHit)
            {
                SetTarget(null);
                return;
            }

            // Currently it only captures the object that is touched
            // Later we will resolve IInteractable on it and apply additional validation.
            SetTarget(hit.collider.gameObject);

            if (m_drawDebug)
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
            }
        }

        private void SetTarget(GameObject newTarget)
        {
            if (ReferenceEquals(m_currentTarget, newTarget))
            {
                return;
            }

            m_currentTarget = newTarget;

            if (m_drawDebug)
            {
                string name = m_currentTarget != null ? m_currentTarget.name : "null";
                Debug.Log($"[InteractionDetector] Target: {name}", this);
            }

            OnTargetChanged?.Invoke(m_currentTarget);
        }

        #endregion
    }
}
