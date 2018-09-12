using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class DaggerSlashBehaviour : AbilityBehaviour
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
            var useParams = GetUseParams(target);

            if (!IsAttacking && GetComponent<PlayerControl>())
            {
                if (PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
                    currentTarget = null;
                }
                else
                {
                    UIManager.Instance.alertMessageController.TriggerAlert(messageType);
                    messageType = CameraUI.AlertMessageType.None;
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
            }
        }

        List<CombatCheck> PopulateCombatChecks()
        {
            if (NoTarget)
            {
                combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(NoTarget, CameraUI.AlertMessageType.NoTarget)
                };
                return combatChecks;
            }
            else
            {
                combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(TargetNotInRange, CameraUI.AlertMessageType.TargetNotInRange),
                    new CombatCheck(TargetNotInLineOfSight, CameraUI.AlertMessageType.TargetNotInLineOfSight),
                    new CombatCheck(CorrectWeaponNotEquipped, CameraUI.AlertMessageType.CorrectWeaponNotEquipped),
                    new CombatCheck(TargetNotAlive, CameraUI.AlertMessageType.TargetNotAlive),
                    new CombatCheck(CharacterNotAlive, CameraUI.AlertMessageType.CharacterNotAlive),
                    new CombatCheck(AbilityOnCooldown, CameraUI.AlertMessageType.AbilityOnCooldown)
                };
                return combatChecks;
            }
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as DaggerSlashConfig).Damage.Value;
            var animationName = (ability as DaggerSlashConfig).AnimationName;
            var reliantStat = (ability as DaggerSlashConfig).ReliantStat;
            var statMultiplier = (ability as DaggerSlashConfig).StatMultiplier;
            var hitAnimationName = (ability as DaggerSlashConfig).HitAnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, null, ability, reliantStat, statMultiplier, hitAnimationName);

            return useParams;
        }

        protected IEnumerator PerformDaggerSlash(AbilityUseParams useParams)
        {
            var enemyHealthController = useParams.target.GetComponent<HealthController>();
            var enemyHitboxAnimator = useParams.target.GetComponentInChildren<HitAnimationController>().GetComponent<Animator>();

            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);
            yield return animationDelayRoutine = StartCoroutine(AnimationDelay(ANIMATION_DELAY, enemyHitboxAnimator, (ability as DaggerSlashConfig).HitAnimationName));
            Character.damageController.DealDamage(useParams);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            StopAttack(ability.AnimationName);
        }

    }
}