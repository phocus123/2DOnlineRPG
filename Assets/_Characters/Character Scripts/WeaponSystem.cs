using UnityEngine;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] Block[] blockArray;
        [Header("Enemy Weapon")]
        [SerializeField] Weapon enemyWeapon;
        [Header("Player Weapons")]
        [SerializeField] Weapon primaryWeapon;
        [SerializeField] GearSlot weaponSlot;

        void Awake()
        {
            if (GetComponent<CharacterController>() is PlayerControl)
            {
                weaponSlot.OnPrimaryWeaponEquipped += SetWeapon;
            }
        }

        const int BLOCK_LAYER = 9;

        public Weapon GetWeapon()
        {
            return enemyWeapon;
        }

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
            if (primaryWeapon != null)
            {
                return weaponType == primaryWeapon.WeaponType;
            }
            return false;
        }

        void SetWeapon(EquippableItem item)
        {
            primaryWeapon = item as Weapon;
        }
    }
}