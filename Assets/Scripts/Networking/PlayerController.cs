using UnityEngine;
using Photon.Pun2;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Game.Networking
{
    /// <summary>
    /// Handles player movement, input, and synchronization across the network.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonTransformView))]
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float groundDrag = 5f;
        [SerializeField] private float airDrag = 2f;
        [SerializeField] private float groundDist = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        
        private CharacterController characterController;
        private PhotonTransformView photonTransformView;
        private Vector3 velocity;
        private float currentSpeed;
        private bool isGrounded;
        
        // Network sync variables
        private Vector3 networkPosition;
        private Quaternion networkRotation;
        private Vector3 networkVelocity;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            photonTransformView = GetComponent<PhotonTransformView>();
            
            if (!photonView.IsMine)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            HandleInput();
            HandleMovement();
            SyncRotation();
        }

        /// <summary>
        /// Processes player input for movement and actions.
        /// </summary>
        private void HandleInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            
            currentSpeed = isSprinting ? sprintSpeed : moveSpeed;
            
            Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;
            
            if (characterController.isGrounded)
            {
                isGrounded = true;
                velocity.y = 0f;
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    velocity.y = jumpForce;
                }
            }
            else
            {
                isGrounded = false;
                velocity.y -= 9.81f * Time.deltaTime;
            }
            
            velocity.x = moveDirection.x * currentSpeed;
            velocity.z = moveDirection.z * currentSpeed;
        }

        /// <summary>
        /// Applies movement and gravity.
        /// </summary>
        private void HandleMovement()
        {
            characterController.Move(velocity * Time.deltaTime);
        }

        /// <summary>
        /// Synchronizes player rotation with mouse input.
        /// </summary>
        private void SyncRotation()
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX * 2f, 0);
        }

        /// <summary>
        /// Photon serialization for network synchronization.
        /// </summary>
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Send position, rotation, and velocity
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
                stream.SendNext(velocity);
            }
            else
            {
                // Receive position, rotation, and velocity
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                networkVelocity = (Vector3)stream.ReceiveNext();
                
                // Interpolate to network position
                if (!photonView.IsMine)
                {
                    transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 5f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 5f);
                }
            }
        }
    }
}
