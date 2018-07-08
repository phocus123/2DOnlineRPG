using UnityEngine;

namespace RPG.Characters
{
    public class BowShotBehaviour : AbilityBehaviour
    {
        //private Castbar castbar;

        private void Start()
        {
            //castbar = FindObjectOfType<Castbar>();
        }

        public override void Use(GameObject target)
        {
            StartAttack(target);
        }

        private void StartAttack(GameObject target)
        {
            var damage = (ability as BowShotConfig).Damage.Value;
            var projectilePrefab = (ability as BowShotConfig).ProjectilePrefab;

            attackRoutine = StartCoroutine(ProjectileAttack(target, damage, projectilePrefab));
        }
    }
}