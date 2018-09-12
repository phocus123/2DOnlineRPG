using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class HealBehaviour : AbilityBehaviour
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
                    attackRoutine = StartCoroutine(CastHealOnTarget(target));
                }
                else
                    UIManager.Instance.alertMessageController.TriggerAlert(messageType);
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(CastHealOnTarget(target));
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

        IEnumerator CastHealOnTarget(GameObject target)
        {
            var player = GetComponent<PlayerControl>();

            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            if (player)
                UIManager.Instance.castbar.TriggerCastBar(ability);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            if (target == null || target.GetComponent<EnemyAI>())
            {
                var targetHitboxAnimator = transform.GetComponentInChildren<HitAnimationController>().GetComponent<Animator>();
                targetHitboxAnimator.SetTrigger((ability as HealConfig).HitAnimationName);
                HealTarget();
            }
            // else heal friendly target
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
        }

        private void HealTarget()
        {
            GetComponent<HealthController>().CurrentHealthPoints += (ability as HealConfig).HealAmount.Value;
            UIManager.Instance.TriggerCombatText(transform.position, (ability as HealConfig).HealAmount.Value, CombatTextType.Heal);
        }
    }
}
