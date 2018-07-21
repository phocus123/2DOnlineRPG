using System;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Slash Attack")]
    public class SlashAttackConfig : Ability
    {
        [Header("Slash Attack data.")]

        [SerializeField] private AbilityStat damage;
        [SerializeField] private AbilityStat critChance;
        [SerializeField] private AbilityStat critEffect;

        public AbilityStat Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public AbilityStat CritChance
        {
            get { return critChance; }
            set { critChance = value; }
        }

        public AbilityStat CritEffect
        {
            get { return critEffect; }
            set { critEffect = value; }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SlashAttackBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SlashAttackUI>();
        }
    }
}