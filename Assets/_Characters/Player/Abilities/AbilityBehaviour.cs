using System.Collections;
using UnityEngine;
using RPG.CameraUI;
using System;
using RPG.Core;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected CharacterAnimationController characterAnimationController;
        protected CharacterMovementController characterMovementController;
        protected WeaponController weaponController;
        protected DamageController damageController;
        protected UIManager uIManager;
        protected Castbar castbar;

        protected Ability ability;
        protected Character currentTarget;
        protected Coroutine animationDelayRoutine;
        protected Coroutine attackRoutine;

        protected const string MeleeTrigger = "MeleeImpact";
        protected const string HealTrigger = "HealImpact";
        protected const float ANIMATION_DELAY = 0.5f;

        Character character;

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }

        protected bool TargetExists { get { return currentTarget != null; } }
        protected bool TargetInRange { get { return weaponController.IsTargetInRange(ability.AbilityRange, currentTarget.gameObject); } }
        protected bool TargetInLineOfSight { get { return weaponController.InLineOfSight(currentTarget.gameObject); } }
        protected bool TargetIsAlive { get { return currentTarget.GetComponent<HealthController>().CurrentHealthPoints > 0; } }
        protected bool CharacterIsAlive { get { return GetComponent<HealthController>().CurrentHealthPoints > 0; } }
        protected bool CorrectWeaponType { get { return weaponController.CorrectWeaponTypeEquipped(ability.WeaponType); } }
        protected bool NotCurrentlyAttacking { get { return !Character.IsAttacking; } }
        protected bool NotCurrentlyMoving { get { return !characterMovementController.IsMoving; } }
        protected bool AbilityCooldownInactive { get { return !ability.CooldownActive; } }

        public event Action<float> OnAttackInitiated = delegate { };
        public abstract void Use(GameObject target);

        protected void TriggerCooldown(float time)
        {
            ability.CooldownActive = true;
            OnAttackInitiated(time);
            StartCoroutine(ResetAbilityAfterCooldown(ability.Cooldown.Value));
        }

        IEnumerator ResetAbilityAfterCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            ability.CooldownActive = false;
        }

        protected IEnumerator AnimationDelay(float seconds, Animator enemyHitboxAnimator, string animationTriggerName)
        {
            yield return new WaitForSeconds(seconds);
            enemyHitboxAnimator.SetTrigger(animationTriggerName);
            StopCoroutine(animationDelayRoutine);
        }

        protected IEnumerator UnregisterProjectileEvent(Projectile attack, DamageController damageController)
        {
            yield return new WaitUntil(() => attack.HasHitTarget);
            attack.InvokeOnHitTarget -= damageController.DealDamage;
        }

        protected void StopAttackIfMoving(Castbar castbar = null)
        {
            if (attackRoutine != null && GetComponent<CharacterMovementController>().IsMoving)
            {
                StopAttack(ability.AnimationName);
            }
        }

        protected void StopAttack(string animationName)
        {
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                GetComponent<CharacterAnimationController>().StopAttackAnimation(animationName);
                var playerCharacter = GetComponent<PlayerControl>();

                if (castbar != null && playerCharacter)
                {
                    castbar.StopCasting();
                }
            }
        }
    }
}