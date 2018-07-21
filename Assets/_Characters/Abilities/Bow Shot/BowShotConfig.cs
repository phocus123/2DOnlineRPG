using UnityEngine;
using System;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Bow Shot")]
    public class BowShotConfig : Ability
    {
        [Header("Bow Shot data.")]
        [SerializeField] AbilityStat damage;
        [SerializeField] AbilityStat critChance;
        [SerializeField] AbilityStat critEffect;
        [SerializeField] GameObject projectilePrefab = null;

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

        public GameObject ProjectilePrefab
        {
            get { return projectilePrefab; }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<BowShotBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<BowShotUI>();
        }
    }
}