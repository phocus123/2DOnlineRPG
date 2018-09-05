using UnityEngine;
using RPG.Core;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters
{
    public class FireballBehaviour : AbilityBehaviour
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
            currentTarget = target.GetComponent<Character>();
            var useParams = GetUseParams(target);

            if (GetComponent<PlayerControl>())
            {
                if (TargetExists && CorrectWeaponType && TargetInRange && NotCurrentlyAttacking && NotCurrentlyMoving && TargetInLineOfSight)
                {
                    castbar.TriggerCastBar(ability);
                    attackRoutine = StartCoroutine(PerformFireball(useParams));
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(PerformFireball(useParams));
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
            characterAnimationController.StartAttackAnimation(ability.AnimationName);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            Projectile attack = Instantiate(useParams.projectilePrefab, Character.ExitPoints[Character.ExitIndex].position, Quaternion.identity).GetComponent<Projectile>();
            attack.Initialize(useParams.target.transform, useParams);
            attack.InvokeOnHitTarget += damageController.DealDamage;
            StopAttack(ability.AnimationName);
            StartCoroutine(UnregisterProjectileEvent(attack, damageController));
        }
    }
}