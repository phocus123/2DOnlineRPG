using System;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Slash Attack")]
    public class SlashAttackConfig : Ability
    {
        [Header("Slash Attack data.")]
        [SerializeField] private AbilityStat damage;
        [SerializeField] private AbilityStat critChance;
        [SerializeField] private AbilityStat critEffect;

        [Header("Character Stats")]
        [SerializeField] private CharacterStat reliantStat;
        [SerializeField] private float statMultiplier;

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

        public CharacterStat ReliantStat
        {
            get { return reliantStat; }
            set { reliantStat = value; }
        }

        public float StatMultiplier
        {
            get { return statMultiplier; }
            set { statMultiplier = value; }
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