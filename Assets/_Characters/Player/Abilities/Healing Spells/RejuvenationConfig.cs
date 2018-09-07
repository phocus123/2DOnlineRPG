using RPG.CameraUI;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Rejuvenation")]
    public class RejuvenationConfig : Ability
    {
        [Header("Rejuvenation")]
        [SerializeField] AbilityStat healAmount;
        [SerializeField] AbilityStat healFrequency;
        [SerializeField] AbilityStat duration;
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

        public AbilityStat HealFrequency
        {
            get { return healFrequency; }
            set { healFrequency = value; }
        }

        public AbilityStat Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public GameObject ProjectilePrefab { get { return projectilePrefab; } }
        public string HitAnimationName { get { return hitAnimationName; } }
        public CharacterStat ReliantStat { get { return reliantStat; } }
        public float StatMultiplier { get { return statMultiplier; } }

        protected override void OnValidate()
        {
            base.OnValidate();

            healAmount.statName = "Heal Amount";
            healFrequency.statName = "Heal Frequency";
            duration.statName = "Duration";
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<RejuvenationBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<RejuvenateUI>();
        }
    }
}