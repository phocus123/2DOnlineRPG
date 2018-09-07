﻿using System.Collections;
using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class HealBehaviour : AbilityBehaviour
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
            StopAttackIfMoving(castbar);
        }

        public override void Use(GameObject target)
        {
            if(target != null)
                currentTarget = target.GetComponent<Character>();

            if (GetComponent<PlayerControl>())
            {
                if (CorrectWeaponType && TargetIsAlive && CharacterIsAlive && NotCurrentlyAttacking && NotCurrentlyMoving && AbilityCooldownInactive)
                {

                    attackRoutine = StartCoroutine(CastHealOnTarget(target));
                }
            }
            else if (GetComponent<EnemyAI>())
            {
                attackRoutine = StartCoroutine(CastHealOnTarget(target));
            }
        }

        IEnumerator CastHealOnTarget(GameObject target)
        {
            var player = GetComponent<PlayerControl>();

            characterAnimationController.StartAbilityAnimation(ability.AnimationName);

            if (player)
                castbar.TriggerCastBar(ability);

            yield return new WaitForSeconds(ability.AbilitySpeed.Value);

            if (target == null || target.GetComponent<EnemyAI>())
            {
                var targetHitboxAnimator = transform.GetChild(0).GetComponent<Animator>();
                targetHitboxAnimator.SetTrigger(HealTrigger);
                HealTarget();
            }
            // else heal friendly target
            TriggerCooldown(ability.Cooldown.Value);
            StopAttack(ability.AnimationName);
        }

        private void HealTarget()
        {
            GetComponent<HealthController>().CurrentHealthPoints += (ability as HealConfig).HealAmount.Value;
            uIManager.TriggerCombatText(transform.position, (ability as HealConfig).HealAmount.Value, CombatTextType.Heal);
        }
    }
}