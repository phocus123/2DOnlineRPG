using System.Collections;
using UnityEngine;
using RPG.Core;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class BasicShotBehaviour : AbilityBehaviour
    {
        void Start()
        {
            if (castbar == null && GetComponent<PlayerControl>())
                castbar = FindObjectOfType<Castbar>();
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
            if (target != null)
                currentTarget = target.GetComponent<Character>();

            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (TargetExists && TargetIsAlive && CharacterIsAlive && CorrectWeaponType && TargetInRange && NotCurrentlyAttacking && NotCurrentlyMoving && TargetInLineOfSight)
                {
                    castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformBasicShot(useParams));
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformBasicShot(useParams));
            }
        }

        AbilityUseParams GetUseParams(GameObject target)
        {
            var damage = (ability as BasicShotConfig).Damage.Value;
            var projectilePrefab = (ability as BasicShotConfig).ProjectilePrefab;
            var animationName = (ability as BasicShotConfig).AnimationName;
            var reliantStat = (ability as BasicShotConfig).ReliantStat;
            var statMultiplier = (ability as BasicShotConfig).StatMultiplier;
            var hitAnimationName = (ability as BasicShotConfig).HitAnimationName;

            AbilityUseParams useParams = new AbilityUseParams(target, damage, projectilePrefab, ability, reliantStat, statMultiplier, hitAnimationName);

            return useParams;
        }

        IEnumerator PerformBasicShot(AbilityUseParams useParams)
        {
            characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            Projectile attack = Instantiate(useParams.projectilePrefab, Character.ExitPoints[Character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams);
            attack.InvokeOnHitTarget += damageController.DealDamage;
            StopAttack(ability.AnimationName);
            StartCoroutine(UnregisterProjectileEvent(attack, damageController));
        }
    }
}