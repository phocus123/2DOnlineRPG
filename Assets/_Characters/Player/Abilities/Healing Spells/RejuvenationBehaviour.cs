using RPG.Core;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class RejuvenationBehaviour : AbilityBehaviour
    {
        void Start()
        {
            if (uIManager == null)
                uIManager = FindObjectOfType<UIManager>();
            if (damageController == null)
                damageController = GetComponent<DamageController>();
            if (weaponController == null)
                weaponController = GetComponent<WeaponController>();
            if (characterMovementController == null)
                characterMovementController = GetComponent<CharacterMovementController>();
            if (characterAnimationController == null)
                characterAnimationController = GetComponent<CharacterAnimationController>();
        }

        private void Update()
        {
            StopAttackIfMoving(castbar);
        }

        public override void Use(GameObject target)
        {
            if (target != null)
                currentTarget = target.GetComponent<Character>();

            if (GetComponent<PlayerControl>())
            {
                if (CorrectWeaponType && TargetIsAlive && CharacterIsAlive && NotCurrentlyAttacking && NotCurrentlyMoving && AbilityCooldownInactive)
                {

                    attackRoutine = StartCoroutine(CastRejuvenationOnTarget(target));
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(CastRejuvenationOnTarget(target));
            }
        }

        IEnumerator CastRejuvenationOnTarget(GameObject target)
        {
            var player = GetComponent<PlayerControl>();

            characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            if (target == null || target.GetComponent<EnemyAI>())
            {
                var targetHitboxAnimator = transform.GetChild(0).GetComponent<Animator>();
                targetHitboxAnimator.SetTrigger(HealTrigger);
                StartCoroutine(TrackFrequency());
            }
            // else heal friendly target
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
        }

        IEnumerator TrackFrequency()
        {
            var abilityConfig = (ability as RejuvenationConfig);
            float count = 0;
            Rejuvenate();

            while (count < abilityConfig.Duration.Value)
            {
                yield return new WaitForSeconds(abilityConfig.HealFrequency.Value);
                count += abilityConfig.HealFrequency.Value;
                Rejuvenate();
            }
        }

        private void Rejuvenate()
        {
            GetComponent<HealthController>().CurrentHealthPoints += (ability as RejuvenationConfig).HealAmount.Value;
            uIManager.TriggerCombatText(transform.position, (ability as RejuvenationConfig).HealAmount.Value, CombatTextType.Heal);
        }
    }
}