using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace FU
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        float vertical;
        float horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            character.animator = GetComponent<Animator>();
        }
        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
           


            if (isSprinting)
            {
                verticalMovement = 2;
            }

            character.animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
            character.animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
        }  

        public virtual void PlayTargetActionAnimation(
            string targetAnimation,
            bool isPerformingAction,
            bool applyRootMotion = true,
            bool canMove = false,
            bool canRotate = false
            )
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            //Can be used to stop character from attempting new actions
            //for example if you damaged and begin perform damaginf animation
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        } 
    }
}

