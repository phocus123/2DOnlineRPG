using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class PoisonShotBehaviour : AbilityBehaviour
    {
        private void Update()
        {
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            if (target != null)
                currentTarget = target.GetComponent<Character>();

            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    GameManager.Instance.castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformPoisonShot(useParams));
                    currentTarget = null;
                }
                else
                    GameManager.Instance.alertMessageController.TriggerAlert(messageType);
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformPoisonShot(useParams));
            }
        }

        List<CombatCheck> PopulateCombatChecks() // TODO Fix bug where deselecting a target will not trigger the no target message again.
        {
            if (NoTarget)
            {
                List<CombatCheck> combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(NoTarget, "NoTarget")
                };
                return combatChecks;
            }
            else
            {
                List<CombatCheck> combatChecks = new List<CombatCheck>
                {
                    new CombatCheck(TargetNotInRange, "TargetNotInRange"),
                    new CombatCheck(TargetNotInLineOfSight, "TargetNotInLineOfSight"),
                    new CombatCheck(CorrectWeaponNotEquipped, "CorrectWeaponNotEquipped"),
                    new CombatCheck(TargetNotAlive, "TargetNotAlive"),
                    new CombatCheck(CharacterNotAlive, "CharacterIsAlive"),
                    new CombatCheck(IsAttacking, "NotCurrentlyAttacking"),
                    new CombatCheck(IsMoving, "NotCurrentlyMoving"),
                    new CombatCheck(AbilityCooldownActive, "AbilityCooldownInactive")
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
            StartCoroutine(TrackDamageFrequency());
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
            StartCoroutine(UnregisterProjectileEvent(attack, Character.damageController));
        }

        IEnumerator TrackDamageFrequency()
        {
            var abilityConfig = (ability as PoisonShotConfig);
            float count = 0;

            while (count < abilityConfig.Duration.Value)
            {
                yield return new WaitForSeconds(abilityConfig.DamageFrequency.Value);
                count += abilityConfig.DamageFrequency.Value;
                DealDamage();
            }
        }

        void DealDamage()
        {
            var damage = (ability as PoisonShotConfig).Damage.Value;
            var enemyHealthController = currentTarget.GetComponent<HealthController>();
            enemyHealthController.TakeDamage(damage);
            GameManager.Instance.uIManager.TriggerCombatText(enemyHealthController.gameObject.transform.position, damage, CombatTextType.NormalDamage);
        }
    }
}