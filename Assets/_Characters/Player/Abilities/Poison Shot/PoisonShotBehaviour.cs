using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class PoisonShotBehaviour : AbilityBehaviour
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
                    UIManager.Instance.castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformPoisonShot(useParams));
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
                attackRoutine = StartCoroutine(PerformPoisonShot(useParams));
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
            var damage = (ability as PoisonShotConfig).Damage.Value;
            var projectilePrefab = (ability as PoisonShotConfig).ProjectilePrefab;
            var animationName = (ability as PoisonShotConfig).AnimationName;
            var reliantStat = (ability as PoisonShotConfig).ReliantStat;
            var statMultiplier = (ability as PoisonShotConfig).StatMultiplier;
            var hitAnimationName = (ability as PoisonShotConfig).HitAnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, hitAnimationName);

            return useParams;
        }

        IEnumerator PerformPoisonShot(AbilityUseParams useParams)
        {
            Character.characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            Projectile attack = Instantiate(useParams.projectilePrefab, Character.ExitPoints[Character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams);
            attack.InvokeOnHitTarget += Character.damageController.DealDamage;
            StartCoroutine(TrackDamageFrequency(useParams.target.GetComponent<Character>()));
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
            StartCoroutine(UnregisterProjectileEvent(attack, Character.damageController));
        }

        IEnumerator TrackDamageFrequency(Character target)
        {
            var abilityConfig = (ability as PoisonShotConfig);
            float count = 0;
            UIManager.Instance.targetDebuffPanel.Init(ability, count, (ability as PoisonShotConfig).Duration.Value);

            while (count < abilityConfig.Duration.Value)
            {
                yield return new WaitForSeconds(abilityConfig.DamageFrequency.Value);
                count += abilityConfig.DamageFrequency.Value;
                DealDamage(target);
            }
        }

        void DealDamage(Character target)
        {
            var damage = (ability as PoisonShotConfig).Damage.Value;
            var enemyHealthController = target.GetComponent<HealthController>();
            var enemyHitboxAnimator = target.GetComponentInChildren<HitAnimationController>().GetComponent<Animator>();

            enemyHealthController.TakeDamage(damage);
            enemyHitboxAnimator.SetTrigger((ability as PoisonShotConfig).HitAnimationName);
            UIManager.Instance.TriggerCombatText(enemyHealthController.gameObject.transform.position, damage, CombatTextType.NormalDamage);
        }
    }
}