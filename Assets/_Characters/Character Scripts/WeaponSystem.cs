using UnityEngine;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] Block[] blockArray;
        [SerializeField] Weapon[] characterWeapons;

        const int BLOCK_LAYER = 9;

        public Weapon GetWeaponAtIndex(int index)
        {
            return characterWeapons[index];
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

        public bool IsTargetInRange(Weapon weapon, GameObject target)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            return distance <= weapon.AttackRange;
        }
    }
}