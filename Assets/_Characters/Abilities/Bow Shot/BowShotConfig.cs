using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Bow Shot")]
    public class BowShotConfig : Ability
    {
        [Header("Bow Shot data.")]
        [SerializeField] AbilityStat damage;
        [SerializeField] float critChance;
        [SerializeField] float critEffect;
        [SerializeField] GameObject projectilePrefab = null;

        public AbilityStat Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public float CritChance
        {
            get { return critChance; }
            set { critChance = value; }
        }

        public float CritEffect
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
    }
}