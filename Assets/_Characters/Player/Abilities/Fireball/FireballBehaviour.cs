using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class FireballBehaviour : AbilityBehaviour
    {
        List<CombatCheck> combatChecks;

        private void Update()
        {
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            if(target != null)
                currentTarget = target.GetComponent<Character>();

            var useParams = GetUseParams(target);

            if (!IsAttacking && GetComponent<PlayerControl>())
            {
                if (!IsAttacking && PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    UIManager.Instance.castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformFireball(useParams));
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
                attackRoutine = StartCoroutine(PerformFireball(useParams));
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
            float damage = (ability as FireballConfig).Damage.Value;
            GameObject projectilePrefab = (ability as FireballConfig).ProjectilePrefab;
            string animationName = (ability as FireballConfig).AnimationName;
            CharacterStat reliantStat = (ability as FireballConfig).ReliantStat;
            float statMultiplier = (ability as FireballConfig).StatMultiplier;
            var hitAnimationName = (ability as FireballConfig).HitAnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, hitAnimationName);

            return useParams;
        }

        IEnumerator PerformFireball(AbilityUseParams useParams)
        {
            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            Projectile attack = Instantiate(useParams.projectilePrefab, Character.ExitPoints[Character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams);
            attack.InvokeOnHitTarget += Character.damageController.DealDamage;
            StopAttack(ability.AnimationName);
            StartCoroutine(UnregisterProjectileEvent(attack, Character.damageController));
        }
    }
}