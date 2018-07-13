using System.Collections;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public GameObject target;
        public float damageToDeal;
        public GameObject projectilePrefab;

        public AbilityUseParams(GameObject target,  float damageToDeal, GameObject projectilePrefab)
        {
            this.target = target;
            this.damageToDeal = damageToDeal;
            this.projectilePrefab = projectilePrefab;
        }
    }

    public class AbilityBehaviour : MonoBehaviour
    {
        protected Coroutine attackRoutine;
        protected Ability ability;

        Character character;
        Castbar castbar;
        const string MELEE_IMPACT = "MeleeImpact";
        const float MELEE_ANIMATION_DELAY = 0.25f;
        Coroutine animationDelayRoutine;

        public virtual void Use(GameObject target)
        {

        }

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        void Update()
        {
            StopAttackIfMoving();
        }

        protected IEnumerator ProjectileAttack(AbilityUseParams useParams)
        {
            character = GetComponent<Character>();
            castbar = FindObjectOfType<Castbar>();
            var enemyAI = GetComponent<EnemyAI>();

            character.StartAttackAnimation(ability.Weapon.AnimationName);

            if (!enemyAI)
            {
                castbar.TriggerCastBar(ability);
            }

            yield return new WaitForSeconds(ability.AttackSpeed);

            Projectile attack = Instantiate(useParams.projectilePrefab, character.ExitPoints[character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams.damageToDeal, ability);
            StopAttack(character, ability.Weapon.AnimationName);
        }

        protected IEnumerator MeleeAttack(AbilityUseParams useParams)
        {
            character = GetComponent<Character>();
            var enemyHealthSystem = useParams.target.GetComponent<HealthSystem>();
            var enemyHitboxAnimator = useParams.target.transform.GetChild(0).GetComponent<Animator>();

            character.StartAttackAnimation(ability.Weapon.AnimationName);
            animationDelayRoutine = StartCoroutine(AnimationDelay(enemyHitboxAnimator));
            enemyHealthSystem.TakeDamage(useParams.damageToDeal);

            yield return new WaitForSeconds(ability.AttackSpeed);

            StopAttack(character, ability.Weapon.AnimationName);
        }

        IEnumerator AnimationDelay(Animator enemyHitboxAnimator)
        {
            yield return new WaitForSeconds(MELEE_ANIMATION_DELAY);
            enemyHitboxAnimator.SetTrigger(MELEE_IMPACT);
            StopCoroutine(animationDelayRoutine);
        }

        void StopAttackIfMoving()
        {
            if (attackRoutine != null && character.IsMoving)
            {
                StopAttack(character, ability.Weapon.AnimationName);
            }
        }

        void StopAttack(Character character, string animationName)
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                character.StopAttackAnimation(animationName);
                var playerCharacter = character.GetComponent<PlayerControl>();

                if (castbar != null && playerCharacter)
                {
                    castbar.StopCasting();
                }
            }
        }
    }
}