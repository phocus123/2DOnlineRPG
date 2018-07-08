using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Fireball")]
    public class FireballConfig : Ability
    {
        [Header("Fireball data.")]
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

        [SerializeField] private GameObject projectilePrefab = null;
        public GameObject ProjectilePrefab
        {
            get { return projectilePrefab; }
        }

        public override AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<FireballBehaviour>();
        }
    }
}
