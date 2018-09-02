using System;
using UnityEngine;

namespace RPG.Characters
{
    public class GearSlot : MonoBehaviour
    {
        [SerializeField] EquipmentType gearSlotEquipmentType;
        [SerializeField] AnimationClip[] baseClips;

        Animator gearSlotAnimator;
        SpriteRenderer spriteRenderer;
        AnimatorOverrideController animatorOverrideController;

        public event Action<EquippableItem> OnPrimaryWeaponEquipped;

        void Awake()
        {
            CharacterAnimationController characterAnimationController = GetComponentInParent<CharacterAnimationController>();
            EquipmentController equipmentController = GetComponentInParent<EquipmentController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            gearSlotAnimator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(gearSlotAnimator.runtimeAnimatorController);

            characterAnimationController.PlayerMoving += SetAnimationDirection;
            characterAnimationController.OnLayerChanged += ActivateLayer;
            characterAnimationController.AttackAnimationChanged += SetAttackAnimation;
            equipmentController.OnItemEquipped += RegisterEquippedItemEvent;
        }

        void Start()
        {
            gearSlotAnimator.runtimeAnimatorController = animatorOverrideController;
        }

        public void AttachClothingAnimations(AnimationClip[] animationClips)
        {
            spriteRenderer.color = Color.white;

            animatorOverrideController["BaseCharacterMaleCastDown"] = animationClips[0];
            animatorOverrideController["BaseCharacterMaleCastLeft"] = animationClips[1];
            animatorOverrideController["BaseCharacterMaleCastRight"] = animationClips[2];
            animatorOverrideController["BaseCharacterMaleCastUp"] = animationClips[3];

            animatorOverrideController["BaseCharacterMaleDeath"] = animationClips[4];

            animatorOverrideController["BaseCharacterMaleIdleDown"] = animationClips[5];
            animatorOverrideController["BaseCharacterMaleIdleLeft"] = animationClips[6];
            animatorOverrideController["BaseCharacterMaleIdleRight"] = animationClips[7];
            animatorOverrideController["BaseCharacterMaleIdleUp"] = animationClips[8];

            animatorOverrideController["BaseCharacterMaleShootDown"] = animationClips[9];
            animatorOverrideController["BaseCharacterMaleShootLeft"] = animationClips[10];
            animatorOverrideController["BaseCharacterMaleShootRight"] = animationClips[11];
            animatorOverrideController["BaseCharacterMaleShootUp"] = animationClips[12];

            animatorOverrideController["BaseCharacterMaleSlashDown"] = animationClips[13];
            animatorOverrideController["BaseCharacterMaleSlashLeft"] = animationClips[14];
            animatorOverrideController["BaseCharacterMaleSlashRight"] = animationClips[15];
            animatorOverrideController["BaseCharacterMaleSlashUp"] = animationClips[16];

            animatorOverrideController["BaseCharacterMaleWalkDown"] = animationClips[17];
            animatorOverrideController["BaseCharacterMaleWalkLeft"] = animationClips[18];
            animatorOverrideController["BaseCharacterMaleWalkRight"] = animationClips[19];
            animatorOverrideController["BaseCharacterMaleWalkUp"] = animationClips[20];
        }

        public void RemoveCurrentAnimations(EquipmentType equipmentType, EquippableItem item)
        {
            animatorOverrideController["BaseCharacterMaleCastDown"] = null;
            animatorOverrideController["BaseCharacterMaleCastLeft"] = null;
            animatorOverrideController["BaseCharacterMaleCastRight"] = null;
            animatorOverrideController["BaseCharacterMaleCastUp"] = null;

            animatorOverrideController["BaseCharacterMaleDeath"] = null;

            animatorOverrideController["BaseCharacterMaleIdleDown"] = null;
            animatorOverrideController["BaseCharacterMaleIdleLeft"] = null;
            animatorOverrideController["BaseCharacterMaleIdleRight"] = null;
            animatorOverrideController["BaseCharacterMaleIdleUp"] = null;

            animatorOverrideController["BaseCharacterMaleShootDown"] = null;
            animatorOverrideController["BaseCharacterMaleShootLeft"] = null;
            animatorOverrideController["BaseCharacterMaleShootRight"] = null;
            animatorOverrideController["BaseCharacterMaleShootUp"] = null;

            animatorOverrideController["BaseCharacterMaleSlashDown"] = null;
            animatorOverrideController["BaseCharacterMaleSlashLeft"] = null;
            animatorOverrideController["BaseCharacterMaleSlashRight"] = null;
            animatorOverrideController["BaseCharacterMaleSlashUp"] = null;

            animatorOverrideController["BaseCharacterMaleWalkDown"] = null;
            animatorOverrideController["BaseCharacterMaleWalkLeft"] = null;
            animatorOverrideController["BaseCharacterMaleWalkRight"] = null;
            animatorOverrideController["BaseCharacterMaleWalkUp"] = null;

            Color c = spriteRenderer.color;
            c.a = 0;
            spriteRenderer.color = c;
            item.ItemUnEquipped -= RemoveCurrentAnimations;
            item.ItemEquipped -= CheckForCorrectEquipmentType;
        }

        void SetAnimationDirection(float x, float y)
        {
            gearSlotAnimator.SetFloat("x", x);
            gearSlotAnimator.SetFloat("y", y);
        }

        void ActivateLayer(string layerName)
        {
            if (gearSlotAnimator != null)
            {
                for (int i = 0; i < gearSlotAnimator.layerCount; i++)
                {
                    gearSlotAnimator.SetLayerWeight(i, 0);
                }

                gearSlotAnimator.SetLayerWeight(gearSlotAnimator.GetLayerIndex(layerName), 1);
            }
        }

        void SetAttackAnimation(string animationName, bool flag)
        {
            gearSlotAnimator.SetBool(animationName, flag);
        }

        void RegisterEquippedItemEvent(EquippableItem item)
        {
            item.ItemEquipped += CheckForCorrectEquipmentType;
        }

        void CheckForCorrectEquipmentType(EquipmentType equipmentType, EquippableItem item)
        {
            if (equipmentType == gearSlotEquipmentType)
            {
                if (equipmentType == EquipmentType.PrimaryWeapon)
                {
                    OnPrimaryWeaponEquipped(item);
                }
                AttachClothingAnimations(item.AnimationClips);
                item.ItemUnEquipped += RemoveCurrentAnimations;
            }
        }
    }
}