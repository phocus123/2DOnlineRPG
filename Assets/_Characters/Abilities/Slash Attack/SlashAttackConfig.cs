using System;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Slash Attack")]
    public class SlashAttackConfig : Ability
    {
        [Header("Slash Attack data.")]

        [SerializeField] private AbilityStat damage;
        public AbilityStat Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        [SerializeField] private float critChance;
        public float CritChance
        {
            get { return critChance; }
            set { critChance = value; }
        }

        [SerializeField] private float critEffect;
        public float CritEffect
        {
            get { return critEffect; }
            set { critEffect = value; }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SlashAttackBehaviour>();
        }
    }
}