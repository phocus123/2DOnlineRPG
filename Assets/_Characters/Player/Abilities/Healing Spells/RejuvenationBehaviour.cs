using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class RejuvenationBehaviour : AbilityBehaviour
    {
        List<CombatCheck> combatChecks;

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
                    UIManager.Instance.alertMessageController.TriggerAlert(messageType);
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(CastRejuvenationOnTarget(target));
            }
        }

        List<CombatCheck> PopulateCombatChecks()
        {
            combatChecks = new List<CombatCheck>
            {
                new CombatCheck(CorrectWeaponNotEquipped, CameraUI.AlertMessageType.CorrectWeaponNotEquipped),
                new CombatCheck(CharacterNotAlive, CameraUI.AlertMessageType.CharacterNotAlive),
                new CombatCheck(AbilityOnCooldown, CameraUI.AlertMessageType.AbilityOnCooldown)
            };

            return combatChecks;
        }

        IEnumerator CastRejuvenationOnTarget(GameObject target)
        {
            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            if (target == null || target.GetComponent<EnemyAI>())
            {
                var targetHitboxAnimator = transform.GetComponentInChildren<HitAnimationController>().GetComponent<Animator>();
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
            UIManager.Instance.playerDebuffPanel.Init(ability, count, (ability as RejuvenationConfig).Duration.Value);

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
            var playerHitboxAnimator = GetComponentInChildren<HitAnimationController>().GetComponent<Animator>();
            playerHitboxAnimator.SetTrigger((ability as RejuvenationConfig).HitAnimationName);
            UIManager.Instance.TriggerCombatText(transform.position, (ability as RejuvenationConfig).HealAmount.Value, CombatTextType.Heal);
        }
    }
}