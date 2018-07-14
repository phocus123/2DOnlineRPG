using UnityEngine;


namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Weapons/Weapon")]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private string animationName = string.Empty;
        [SerializeField] private bool isProjectile = false;
        [SerializeField] private float attackRange;

        public GameObject WeaponObject
        {
            get
            {
                return weaponPrefab;
            }
        }

        public string AnimationName
        {
            get { return animationName; }
        }

        public bool IsProjectile
        {
            get { return isProjectile; }
        }

        public float AttackRange
        {
            get
            {
                return attackRange;
            }

            set
            {
                attackRange = value;
            }
        }
    }
}