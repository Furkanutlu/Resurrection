using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace FU
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        public PlayerManager player;

        private PlayerControls playerControls;

        [Header("Camera Input")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraverticalInput;
        public float camerahorizontalInput;

        [Header("Movement Input")]
        [SerializeField] private Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;

        // New variables
        private bool isRightMousePressed = false;
        private Vector2 mouseInput;
        [SerializeField] private float mouseSensitivity = 0.1f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            // Run this logic when the scene changes
            SceneManager.activeSceneChanged += OnSceneChange;

            // Initially disable controls
            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // If we are loading into our world scene, enable the player's controls
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                // Movement Input
                playerControls.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.Player.Move.canceled += i => movementInput = Vector2.zero;

                // Camera Movement Input
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.canceled += i => cameraInput = Vector2.zero;

                // Right Mouse Button (RMB) Input
                playerControls.PlayerCamera.RightMouseButton.performed += i => isRightMousePressed = true;
                playerControls.PlayerCamera.RightMouseButton.canceled += i => isRightMousePressed = false;

                // Mouse Delta Input (Mouse movement)
                playerControls.PlayerCamera.MouseDelta.performed += i => mouseInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.MouseDelta.canceled += i => mouseInput = Vector2.zero;

                // Dodge Input
                playerControls.PlayerAction.Dodge.performed += i => dodgeInput = true; // Set dodge input

                // Sprint Input
                playerControls.PlayerAction.Sprint.performed += i => sprintInput = true; //holding
                playerControls.PlayerAction.Sprint.canceled += i => sprintInput = false;

                // Jump Input
                playerControls.PlayerAction.Jump.performed += i => jumpInput = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            // Unsubscribe from the scene change event when destroying this object
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleMouseCameraInput();
            HandleDodgeInput();
            HandleSprintingInput();
            HandleJumpInput();
        }

        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // Calculate move amount
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            if (player == null)
            {
                return;
            }

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        }

        private void HandleCameraMovementInput()
        {
            if (!isRightMousePressed)
            {
                cameraverticalInput = cameraInput.y;
                camerahorizontalInput = cameraInput.x;
            }
            else
            {
                cameraverticalInput = 0;
                camerahorizontalInput = 0;
            }
        }

        private void HandleMouseCameraInput()
        {
            if (isRightMousePressed)
            {
                camerahorizontalInput += mouseInput.x * mouseSensitivity;
                cameraverticalInput += mouseInput.y * mouseSensitivity;
            }
        }

        private void HandleDodgeInput()
        {
            if (!dodgeInput) return; // If dodgeInput is false, do nothing

            // Reset dodgeInput to prevent continuous dodging
            dodgeInput = false;

            // Call dodge function in player locomotion manager
            if (player != null)
            {
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintingInput()
        {
            if (sprintInput)
            {
                //handle sprinting
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
    }
}