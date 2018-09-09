using RPG.CameraUI;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Poison Shot")]
    public class PoisonShotConfig : Ability
    {
        [Header("Rejuvenation")]
        [SerializeField] AbilityStat damage;
        [SerializeField] AbilityStat damageFrequency;
        [SerializeField] AbilityStat duration;
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

        public AbilityStat DamageFrequency
        {
            get { return damageFrequency; }
            set { damageFrequency = value; }
        }

        public GameObject ProjectilePrefab { get { return projectilePrefab; } }
        public string HitAnimationName { get { return hitAnimationName; } }
        public CharacterStat ReliantStat { get { return reliantStat; } }
        public float StatMultiplier { get { return statMultiplier; } }

        public AbilityStat Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            damage.statName = "Damage";
            damageFrequency.statName = "Damage Frequency";
            duration.statName = "Duration";
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<PoisonShotBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            throw new System.NotImplementedException();
        }
    }
}