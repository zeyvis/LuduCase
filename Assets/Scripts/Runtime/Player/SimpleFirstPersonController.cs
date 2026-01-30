using UnityEngine;

namespace LuduCase.Runtime.Player
{
    /// <summary>
    /// A simple First Person Controller for testing interaction systems.
    /// Handles movement (WASD) and camera rotation (Mouse).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class SimpleFirstPersonController : MonoBehaviour
    {
        #region Inspector

        [Header("Movement Settings")]
        [SerializeField] private float m_walkSpeed = 5f;
        [SerializeField] private float m_gravity = -9.81f;

        [Header("Look Settings")]
        [SerializeField] private float m_mouseSensitivity = 2f;
        [SerializeField] private Transform m_cameraTransform;
        [SerializeField] private float m_lookXLimit = 85f;

        #endregion

        #region Fields

        private CharacterController m_characterController;
        private Vector3 m_velocity;
        private float m_rotationX = 0f;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            m_characterController = GetComponent<CharacterController>();
            if (m_characterController == null)
            {
                Debug.LogError("[SimpleFirstPersonController] CharacterController component is missing. Movement will not work.", this);
            }


            if (m_cameraTransform == null)
            {
                if (Camera.main != null)
                {
                    m_cameraTransform = Camera.main.transform;
                }
                else
                {
                    Debug.LogError("[SimpleFirstPersonController] CameraTransform is not assigned and no Main Camera found. Look rotation will not work.", this);
                }
            }



            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        #endregion

        #region Private Methods

        private void HandleMovement()
        {

            if (m_characterController.isGrounded && m_velocity.y < 0)
            {
                m_velocity.y = -2f;
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");


            Vector3 move = transform.right * moveX + transform.forward * moveZ;


            m_characterController.Move(move * m_walkSpeed * Time.deltaTime);

            m_velocity.y += m_gravity * Time.deltaTime;
            m_characterController.Move(m_velocity * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (m_cameraTransform == null)
            {
                Debug.LogWarning("[SimpleFirstPersonController] CameraTransform is null. Rotation skipped.", this);
                return;
            }


            float mouseX = Input.GetAxis("Mouse X") * m_mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * m_mouseSensitivity;

            m_rotationX -= mouseY;
            m_rotationX = Mathf.Clamp(m_rotationX, -m_lookXLimit, m_lookXLimit);
            m_cameraTransform.localRotation = Quaternion.Euler(m_rotationX, 0, 0);

            transform.Rotate(Vector3.up * mouseX);
        }

        #endregion
    }
}