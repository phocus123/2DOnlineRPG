using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Heal")]
    public class HealConfig : Ability
    {
        [Header("Heal")]
        [SerializeField] AbilityStat healAmount;
        [SerializeField] AbilityStat critChance;
        [SerializeField] AbilityStat critEffect;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] string hitAnimationName;

        [Header("Character Stats")]
        [SerializeField] CharacterStat reliantStat;
        [SerializeField] float statMultiplier;

        public AbilityStat HealAmount
        {
            get { return healAmount; }
            set { healAmount = value; }
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

        void OnValidate()
        {
            healAmount.statName = "Heal Amount";
            critChance.statName = "Critical Chance";
            critEffect.statName = "Critical Effect";
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<HealBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<HealUI>();
        }
    }
}