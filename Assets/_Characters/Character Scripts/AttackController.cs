using RPG.CameraUI;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class AttackController : MonoBehaviour
    {
        CharacterManager characterManager;
        CharacterAnimationController characterAnimationController;
        DamageController damageController;
        Ability ability;
        Castbar castbar;

        Coroutine attackRoutine;
        Coroutine animationDelayRoutine;
        const string MELEE_IMPACT = "MeleeImpact";
        const float MELEE_ANIMATION_DELAY = 0.25f;
        const float GLOBAL_COOLDOWN_AMOUNT = 0.75f;

        public event Action<float> OnAttackInitiated = delegate { };

        void Start()
        {
            damageController = GetComponent<DamageController>();    
            characterManager = GetComponent<CharacterManager>();
            characterAnimationController = GetComponent<CharacterAnimationController>();
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
            var player = GetComponent<PlayerControl>();

            characterAnimationController.StartAttackAnimation(ability.AnimationName);

            if (player)
            {
                castbar.TriggerCastBar(ability);
            }

            yield return new WaitForSeconds(ability.AttackSpeed.Value);

            OnAttackInitiated(GLOBAL_COOLDOWN_AMOUNT);

            Projectile attack = Instantiate(useParams.projectilePrefab, characterManager.ExitPoints[characterManager.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams);
            attack.InvokeOnHitTarget += damageController.DealDamage;
            StopAttack(ability.AnimationName);

            yield return new WaitForSeconds(GLOBAL_COOLDOWN_AMOUNT);

            attack.InvokeOnHitTarget -= damageController.DealDamage;
        }

        protected IEnumerator MeleeAttack(AbilityUseParams useParams)
        {
            var enemyHitboxAnimator = useParams.target.transform.GetChild(0).GetComponent<Animator>();

            characterAnimationController.StartAttackAnimation(ability.AnimationName);
            animationDelayRoutine = StartCoroutine(AnimationDelay(enemyHitboxAnimator));
            damageController.DealDamage(useParams);
            OnAttackInitiated(ability.AttackSpeed.Value + GLOBAL_COOLDOWN_AMOUNT);

            yield return new WaitForSeconds(ability.AttackSpeed.Value + GLOBAL_COOLDOWN_AMOUNT);

            StopAttack(ability.AnimationName);
        }

        IEnumerator AnimationDelay(Animator enemyHitboxAnimator)
        {
            yield return new WaitForSeconds(MELEE_ANIMATION_DELAY);
            enemyHitboxAnimator.SetTrigger(MELEE_IMPACT);
            StopCoroutine(animationDelayRoutine);
        }

        void StopAttackIfMoving()
        {
            if (attackRoutine != null && GetComponent<CharacterMovementController>().IsMoving)
            {
                StopAttack(ability.AnimationName);
            }
        }

        void StopAttack(string animationName)
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                characterAnimationController.StopAttackAnimation(animationName);
                var playerCharacter = GetComponent<PlayerControl>();

                if (castbar != null && playerCharacter)
                {
                    castbar.StopCasting();
                }
            }
        }
    }
}