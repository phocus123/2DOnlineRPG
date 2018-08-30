using RPG.CameraUI;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class AttackSystem : MonoBehaviour
    {
        public delegate void OnAttackInitiated(float time);
        public event OnAttackInitiated InvokeOnAttackInitiated;

        Ability ability;
        Character character;
        Castbar castbar;
        DamageSystem damageSystem;

        Coroutine attackRoutine;
        Coroutine animationDelayRoutine;
        const string MELEE_IMPACT = "MeleeImpact";
        const float MELEE_ANIMATION_DELAY = 0.25f;
        const float GLOBAL_COOLDOWN_AMOUNT = 0.75f;

        void Start()
        {
            damageSystem = GetComponent<DamageSystem>();    
            character = GetComponent<Character>();
        }

        void Update()
        {
            StopAttackIfMoving();
        }

        public void Attack(AbilityUseParams abilityUseParams)
        {
            ability = abilityUseParams.ability;
            if (abilityUseParams.projectilePrefab == null)
            {
                attackRoutine = StartCoroutine(MeleeAttack(abilityUseParams));
            }
            else
            {
                attackRoutine = StartCoroutine(ProjectileAttack(abilityUseParams));
            }
        }

        protected IEnumerator ProjectileAttack(AbilityUseParams useParams)
        {
            castbar = FindObjectOfType<Castbar>();
            var enemyAI = GetComponent<EnemyAI>();

            character.StartAttackAnimation(ability.AnimationName);

            if (!enemyAI)
            {
                castbar.TriggerCastBar(ability);
            }

            yield return new WaitForSeconds(ability.AttackSpeed.Value);

            if (InvokeOnAttackInitiated != null)
            {
                InvokeOnAttackInitiated(GLOBAL_COOLDOWN_AMOUNT);
            }

            Projectile attack = Instantiate(useParams.projectilePrefab, character.ExitPoints[character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.InvokeOnHitTarget += damageSystem.DealDamage;
            attack.Initialize(useParams.target.transform, useParams);
            StopAttack(character, ability.AnimationName);

            yield return new WaitForSeconds(GLOBAL_COOLDOWN_AMOUNT);

            attack.InvokeOnHitTarget -= damageSystem.DealDamage;
        }

        protected IEnumerator MeleeAttack(AbilityUseParams useParams)
        {
            var enemyHitboxAnimator = useParams.target.transform.GetChild(0).GetComponent<Animator>();

            character.StartAttackAnimation(ability.AnimationName);
            animationDelayRoutine = StartCoroutine(AnimationDelay(enemyHitboxAnimator));
            damageSystem.DealDamage(useParams);

            if (InvokeOnAttackInitiated != null)
            {
                InvokeOnAttackInitiated(ability.AttackSpeed.Value + GLOBAL_COOLDOWN_AMOUNT);
            }

            yield return new WaitForSeconds(ability.AttackSpeed.Value + GLOBAL_COOLDOWN_AMOUNT);

            StopAttack(character, ability.AnimationName);
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
                StopAttack(character, ability.AnimationName);
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