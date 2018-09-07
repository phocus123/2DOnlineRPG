using System.Collections;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class DaggerSlashBehaviour : AbilityBehaviour
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
            StopAttackIfMoving();
        }

        public override void Use(GameObject target)
        {
            currentTarget = target.GetComponent<Character>();
            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (TargetExists && TargetIsAlive && CharacterIsAlive && CorrectWeaponType && TargetInRange && NotCurrentlyAttacking && NotCurrentlyMoving && TargetInLineOfSight)
                {
                    attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformDaggerSlash(useParams));
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
            var enemyHitboxAnimator = useParams.target.transform.GetChild(0).GetComponent<Animator>();

            characterAnimationController.StartAbilityAnimation(ability.AnimationName);
            yield return animationDelayRoutine = StartCoroutine(AnimationDelay(ANIMATION_DELAY, enemyHitboxAnimator, MeleeTrigger));
            damageController.DealDamage(useParams);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            StopAttack(ability.AnimationName);
        }

    }
}