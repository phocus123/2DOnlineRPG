using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected Coroutine attackRoutine;
        protected Ability ability;

        Character character;

        public Ability Ability
        {
            set { ability = value; }
        }

        void Start()
        {
            character = GetComponent<Character>();
        }

        void Update()
        {
            StopAttackIfMoving();
        }

        private void StopAttackIfMoving()
        {
            if (attackRoutine != null && character.IsMoving)
            {
                StopAttack(character);
            }
        }

        public abstract void Use(GameObject target);

        protected IEnumerator ProjectileAttack(GameObject target, float damageToDeal, GameObject projectilePrefab)
        {
            var enemyHealthSystem = target.GetComponent<HealthSystem>();
            character = GetComponent<Character>();

            character.StartAttackAnimation();
            //castbar.TriggerCastBar(useParams);

            yield return new WaitForSeconds(ability.AttackSpeed);

            Projectile attack = Instantiate(projectilePrefab, character.ExitPoints[character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(target.transform, damageToDeal, ability);
           
            //player.Energy.CurrentValue -= config.Energy;
            StopAttack(character);
        }

        void StopAttack(Character character)
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                //castbar.StopCasting();
                character.StopAttackAnimation();
            }
        }
    }
}