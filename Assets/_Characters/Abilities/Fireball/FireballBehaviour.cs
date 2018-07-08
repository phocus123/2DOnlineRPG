using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class FireballBehaviour : AbilityBehaviour
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
            var damage = (ability as FireballConfig).Damage.Value;
            var projectilePrefab = (ability as FireballConfig).ProjectilePrefab;

            attackRoutine = StartCoroutine(ProjectileAttack(target, damage, projectilePrefab));
        }
    }
}