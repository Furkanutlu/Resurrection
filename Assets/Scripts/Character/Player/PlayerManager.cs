using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;

        protected override void Awake()
        {
            base.Awake();
            // do more stuff, only for the player

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
           playerStatsManager = GetComponent<PlayerStatsManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsOwner)
            {
                return;
            }

            //Handle movement
            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {

            if (!IsOwner)
            {
                return;
            }
            

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();

        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();


            if (PlayerUIManager.instance == null)
            {
                Debug.Log("UI Manager BULUNAMADI!");
            }
            else
            {
                Debug.Log("UI Manager VAR!");
                if (PlayerUIManager.instance.playerUIHudManager == null)
                    Debug.Log("playerUIHudManager null!");
                else
                    Debug.Log("playerUIHudManager da var!");
            }

            if (PlayerUIManager.instance == null)
            {
                PlayerUIManager.instance = FindObjectOfType<PlayerUIManager>();
            }

            //if this is the player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;

                if (PlayerUIManager.instance != null &&
    PlayerUIManager.instance.playerUIHudManager != null)
                {
                    playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                }
                

                playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStamaninaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStamaninaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
                

                if (PlayerUIManager.instance != null &&
PlayerUIManager.instance.playerUIHudManager != null)
                {
                    PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
                }
            }
        }
    }
}

