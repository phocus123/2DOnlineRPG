using RPG.CameraUI;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Fireball")]
    public class FireballConfig : Ability
    {
        [Header("Fireball")]
        public AbilityStat Damage;
        public AbilityStat CritChance;
        public AbilityStat CritEffect;
        public GameObject ProjectilePrefab;
        public string AnimationTrigger;

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
            return objectToAttachTo.AddComponent<FireballBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<FireballUI>();
        }
    }
}
