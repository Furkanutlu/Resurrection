using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FU
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public int CalculateStamaninaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;

            stamina = endurance * 10;

            return Mathf.RoundToInt(stamina);
        }

    }
}

