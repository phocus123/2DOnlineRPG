using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Basic Shot")]
    public class BasicShotConfig : Ability
    {
        [Header("Basic Shot")]
        public AbilityStat Damage;
        public AbilityStat CritChance;
        public AbilityStat CritEffect;
        public GameObject ProjectilePrefab;
        public string AnimationTrigger;

        [Header("Character Stats")]
        public CharacterStat ReliantStat;
        public float StatMultiplier;

        void OnValidate()
        {
            Damage.statName = "Damage";
            CritChance.statName = "Critical Chance";
            CritEffect.statName = "Critical Effect";
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<BasicShotBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<BasicShotUI>();
        }
    }
}