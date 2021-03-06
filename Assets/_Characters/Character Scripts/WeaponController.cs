﻿using UnityEngine;

namespace RPG.Characters
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] Block[] blockArray;
        [Header("Player Weapons")]
        [SerializeField] Weapon primaryWeapon;//TODO These don't really need to be serialized.
        [SerializeField] GearSlot weaponSlot;

        void Awake()
        {
            if (GetComponent<PlayerControl>())
            {
                weaponSlot.OnPrimaryWeaponEquipped += SetWeapon;
                weaponSlot.OnPrimaryWeaponUnEquipped += ClearWeapon;
            }
        }

        const int BLOCK_LAYER = 9;

        public void Block(int exitIndex)
        {
            foreach (Block b in blockArray)
            {
                b.Deactivate();
            }

            blockArray[exitIndex].Activate();
        }

        public bool InLineOfSight(GameObject target)
        {
            if (target != null)
            {
                Vector3 targetDirection = (target.transform.position - transform.position);
                LayerMask defaultLayer = 1 << BLOCK_LAYER;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), defaultLayer);
                
                if (hit.collider == null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsTargetInRange(float attackRange, GameObject target)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            return distance <= attackRange;
        }

        public bool CorrectWeaponTypeEquipped(WeaponType weaponType)
        {
            if (weaponType != WeaponType.None)
            {
                if (primaryWeapon != null)
                {
                    return weaponType == primaryWeapon.WeaponType;
                }
            }
            else
                return true;

            return false;
        }

        void SetWeapon(EquippableItem item)
        {
            primaryWeapon = item as Weapon;
        }

        void ClearWeapon()
        {
            primaryWeapon = null;
        }
    }
}