using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace FU
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenAmount = 2;
        private float staminaRegenerationTimer = 0;
        private float staminaTickTimer = 0;
        float staminaRegenerationDelay = 1;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        public int CalculateStamaninaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkManager.isSprinting.Value)
                return;

            if (character.isPerformingAction)
                return;

            staminaRegenerationTimer += Time.deltaTime;

            if (staminaRegenerationTimer >= staminaRegenerationDelay)
            {
                if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
                {
                    staminaTickTimer += Time.deltaTime;

                    if (staminaTickTimer >= 0.1f)
                    {
                        staminaTickTimer = 0;

                        // İvmeli regenerasyon
                        float regenRate = staminaRegenAmount * (1 + (staminaRegenerationTimer - staminaRegenerationDelay));
                        character.characterNetworkManager.currentStamina.Value = Mathf.Clamp(
                            character.characterNetworkManager.currentStamina.Value + regenRate,
                            0,
                            character.characterNetworkManager.maxStamina.Value
                        );

                        // Eğer stamina maks seviyeye ulaştıysa ivmeyi sıfırla
                        if (character.characterNetworkManager.currentStamina.Value >= character.characterNetworkManager.maxStamina.Value)
                        {
                            ResetStaminaRegen();
                        }
                    }
                }
            }
        }

        private void ResetStaminaRegen()
        {
            staminaRegenerationTimer = 0; // Zamanlayıcı sıfırla
            staminaTickTimer = 0;         // Tick zamanlayıcı sıfırla
        }


        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
        {
            

            if (currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenerationTimer = 0;
            }
        }
    }
}

