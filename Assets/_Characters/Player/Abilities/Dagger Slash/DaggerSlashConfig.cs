using System;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Dagger Slash")]
    public class DaggerSlashConfig : Ability
    {
        [Header("Dagger Slash")]
        [SerializeField] AbilityStat damage;
        [SerializeField] AbilityStat critChance;
        [SerializeField] AbilityStat critEffect;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] string hitAnimationName;

        [Header("Character Stats")]
        [SerializeField] CharacterStat reliantStat;
        [SerializeField] float statMultiplier;

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

        public GameObject ProjectilePrefab { get { return projectilePrefab; } }
        public string HitAnimationName { get { return hitAnimationName; } }
        public CharacterStat ReliantStat { get { return reliantStat; } }
        public float StatMultiplier { get { return statMultiplier; } }

        protected override void OnValidate()
        {
            base.OnValidate();

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