using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] RuntimeAnimatorController runtimeAnimatorController;

        Character character;
        CharacterMovementController characterMovementController;
        HealthController healthController;
        Animator animator;

        public delegate void OnAttackAnimationTriggered(string animationName, bool flag);
        public event OnAttackAnimationTriggered AttackAnimationChanged;
        public delegate void OnPlayerMoving(float x, float y);
        public event OnPlayerMoving PlayerMoving;
        public event Action<string> OnLayerChanged;

        public RuntimeAnimatorController RuntimeAnimatorController { get { return runtimeAnimatorController; } }

        void Awake()
        {
            character = GetComponent<Character>();
            characterMovementController = GetComponent<CharacterMovementController>();
            healthController = GetComponent<HealthController>();

            AddRequiredAnimatorComponent();
            healthController.OnCharacterDeath += TriggerDeathAnimation;
        }

        void Update()
        {
            HandleLayers();
        }

        public void StartAbilityAnimation(string animationName)
        {
            character.IsAttacking = true;
            animator.SetBool(animationName, character.IsAttacking);

            if (GetComponent<PlayerControl>())
            {
                AttackAnimationChanged(animationName, character.IsAttacking);
            }
        }

        public void StopAttackAnimation(string animationName)
        {
            character.IsAttacking = false;
            animator.SetBool(animationName, character.IsAttacking);

            if (GetComponent<PlayerControl>())
            {
                AttackAnimationChanged(animationName, character.IsAttacking);
            }
        }
        
        void ActivateLayer(string layerName)
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                animator.SetLayerWeight(i, 0);
            }

            animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);

            if (GetComponent<PlayerControl>())
            {
                OnLayerChanged(layerName);
            }
        }

        void AddRequiredAnimatorComponent()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;
        }

        void HandleLayers()
        {
            if (character.IsAlive)
            {
                if (characterMovementController.IsMoving)
                {
                    ActivateLayer("WalkLayer");

                    animator.SetFloat("x", characterMovementController.Direction.x);
                    animator.SetFloat("y", characterMovementController.Direction.y);

                    if (GetComponent<PlayerControl>())
                    {
                        PlayerMoving(characterMovementController.Direction.x, characterMovementController.Direction.y);
                    }
                }
                else if (character.IsAttacking)
                {
                    ActivateLayer("AttackLayer");
                }
                else
                {
                    ActivateLayer("IdleLayer");
                }
            }
            else
            {
                ActivateLayer("DeathLayer");
            }
        }

        void TriggerDeathAnimation()
        {
            healthController.OnCharacterDeath -= TriggerDeathAnimation;
            animator.SetTrigger("Die");
        }
    }
}