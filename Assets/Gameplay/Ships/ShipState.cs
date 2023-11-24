using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Ships
{
    [System.Serializable]
    public struct ShipState
    {
        [Range(0, 1)]
        [SerializeField] float _healthLostPercentThreshold;
        [SerializeField] ShipSkin _shipSkin;
        public ShipSkin ShipSkin => _shipSkin;
        public float HealthLostPercentThreshold => _healthLostPercentThreshold;
    }

    public static class ShipExtensions
    {
        public static int GetDeteriotaionStateIndex(this List<ShipState> deteriorationStates, float currentHealth, float maxHealth)
        {
            float healthLostPercent = 1 - (currentHealth / maxHealth);
            for (int i = deteriorationStates.Count - 1; i >= 0; i--)
            {
                if (healthLostPercent >= deteriorationStates[i].HealthLostPercentThreshold)
                    return i;
            }
            return -1;
        }     
    }
}
