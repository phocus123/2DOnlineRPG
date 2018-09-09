using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class RejuvenationBehaviour : AbilityBehaviour
    {
        private void Update()
        {
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            if (target != null)
                currentTarget = target.GetComponent<Character>();

            if (GetComponent<PlayerControl>())
            {
                if (PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    attackRoutine = StartCoroutine(CastRejuvenationOnTarget(target));
                }
                else
                    GameManager.Instance.alertMessageController.TriggerAlert(messageType);
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(CastRejuvenationOnTarget(target));
            }
        }

        List<CombatCheck> PopulateCombatChecks()
        {
            List<CombatCheck> combatChecks = new List<CombatCheck>
            {
                new CombatCheck(CorrectWeaponNotEquipped, "CorrectWeaponType"),
                new CombatCheck(CharacterNotAlive, "CharacterIsAlive"),
                new CombatCheck(IsAttacking, "NotCurrentlyAttacking"),
                new CombatCheck(IsMoving, "NotCurrentlyMoving"),
                new CombatCheck(AbilityCooldownActive, "AbilityCooldownActive")
            };

            return combatChecks;
        }

        IEnumerator CastRejuvenationOnTarget(GameObject target)
        {
            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            if (target == null || target.GetComponent<EnemyAI>())
            {
                var targetHitboxAnimator = transform.GetChild(0).GetComponent<Animator>();
                targetHitboxAnimator.SetTrigger((ability as RejuvenationConfig).HitAnimationName);
                StartCoroutine(TrackHealFrequency());
            }
            // else heal friendly target
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
        }

        IEnumerator TrackHealFrequency()
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
            GameManager.Instance.uIManager.TriggerCombatText(transform.position, (ability as RejuvenationConfig).HealAmount.Value, CombatTextType.Heal);
        }
    }
}