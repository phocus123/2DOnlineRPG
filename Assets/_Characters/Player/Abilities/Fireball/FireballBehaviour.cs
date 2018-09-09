using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;

namespace RPG.Characters
{
    public class FireballBehaviour : AbilityBehaviour
    {
        private void Update()
        {
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            if(target != null)
                currentTarget = target.GetComponent<Character>();

            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (PerformCombatChecks(out messageType, PopulateCombatChecks()))
                {
                    GameManager.Instance.castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformFireball(useParams));
                    currentTarget = null;
                }
                else
                    GameManager.Instance.alertMessageController.TriggerAlert(messageType);
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