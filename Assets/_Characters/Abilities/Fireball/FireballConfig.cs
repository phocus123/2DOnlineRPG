using RPG.CameraUI;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Fireball")]
    public class FireballConfig : Ability
    {
        [Header("Fireball data.")]
        [SerializeField] AbilityStat damage;
        [SerializeField] AbilityStat critChance;
        [SerializeField] private AbilityStat critEffect;
        [SerializeField] private GameObject projectilePrefab = null;

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

        public GameObject ProjectilePrefab
        {
            get { return projectilePrefab; }
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
            return objectToAttachTo.AddComponent<FireballBehaviour>();
        }

        public override AbilityUI GetUIComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<FireballUI>();
        }
    }
}
