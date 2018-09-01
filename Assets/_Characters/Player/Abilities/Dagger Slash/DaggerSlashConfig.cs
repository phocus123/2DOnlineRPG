using System;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Dagger Slash")]
    public class DaggerSlashConfig : Ability
    {
        [Header("Dagger Slash")]
        public AbilityStat Damage;
        public AbilityStat CritChance;
        public AbilityStat CritEffect;

        [Header("Character Stats")]
        public PrimaryStat ReliantStat;
        public float StatMultiplier;

        void OnValidate()
        {
            Damage.statName = "Damage";
            CritChance.statName = "Critical Chance";
            CritEffect.statName = "Critical Effect";
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<DaggerSlashBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<DaggerSlashUI>();
        }
    }
}