using System.Collections;
using UnityEngine;
using RPG.CameraUI;
using System;
using RPG.Core;
using System.Collections.Generic;

namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected Ability ability;
        protected Character currentTarget;
        protected Coroutine animationDelayRoutine;
        protected Coroutine attackRoutine;

        protected const float ANIMATION_DELAY = 0.5f;
        protected AlertMessageType messageType;

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

        protected bool NoTarget { get { return currentTarget == null; } }
        protected bool TargetNotInRange { get { return !character.weaponController.IsTargetInRange(ability.AbilityRange, currentTarget.gameObject); } }
        protected bool TargetNotInLineOfSight { get { return !character.weaponController.InLineOfSight(currentTarget.gameObject); } }
        protected bool TargetNotAlive { get { return currentTarget.GetComponent<HealthController>().CurrentHealthPoints == 0; } }
        protected bool CharacterNotAlive { get { return GetComponent<HealthController>().CurrentHealthPoints == 0; } }
        protected bool CorrectWeaponNotEquipped { get { return !character.weaponController.CorrectWeaponTypeEquipped(ability.WeaponType); } }
        protected bool IsAttacking { get { return Character.IsAttacking; } }
        protected bool IsMoving { get { return character.characterMovementController.IsMoving; } }
        protected bool AbilityCooldownActive { get { return ability.CooldownActive; } }

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

        protected void StopAttackIfMoving()
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

                if (playerCharacter)
                {
                    GameManager.Instance.castbar.StopCasting();
                }
            }
        }


        protected bool PerformCombatChecks(out AlertMessageType messageType, List<CombatCheck> combatChecks)
        {
            messageType = AlertMessageType.None;

            for (int i = 0; i < combatChecks.Count; i++)
            {
                if (combatChecks[i].name == "NoTarget" && NoTarget)
                {
                    messageType = AlertMessageType.NoTarget;
                    return false;
                }

                if (combatChecks[i].name == "TargetNotInRange" && TargetNotInRange)
                {
                    messageType = AlertMessageType.TargetNotInRange;
                    return false;
                }

                if (combatChecks[i].name == "TargetNotInLineOfSight" && TargetNotInLineOfSight)
                {
                    messageType = AlertMessageType.TargetNotInLineOfSight;
                    return false;
                }

                if (combatChecks[i].name == "TargetNotAlive" && TargetNotAlive)
                {
                    messageType = AlertMessageType.TargetNotAlive;
                    return false;
                }

                if (combatChecks[i].name == "CharacterNotAlive" && CharacterNotAlive)
                {
                    messageType = AlertMessageType.CharacterNotAlive;
                    return false;
                }

                if (combatChecks[i].name == "CorrectWeaponNotEquipped" && CorrectWeaponNotEquipped)
                {
                    messageType = AlertMessageType.CorrectWeaponNotEquipped;
                    return false;
                }

                if (combatChecks[i].name == "AbilityCooldownActive" && AbilityCooldownActive)
                {
                    messageType = AlertMessageType.AbilityOnCooldown;
                    return false;
                }

                if (combatChecks[i].name == "IsAttacking" && IsAttacking)
                    return false;


                if (combatChecks[i].name == "IsMoving" && IsMoving)
                    return false;
            }
            return true;
        }
    }

    public struct CombatCheck
    {
        public bool combatCheck;
        public string name;

        public CombatCheck(bool combatCheck, string name)
        {
            this.combatCheck = combatCheck;
            this.name = name;
        }
    }
}