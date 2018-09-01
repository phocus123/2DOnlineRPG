using UnityEngine;


namespace RPG.Characters
{
    public enum WeaponType
    {
        Dagger,
        Bow,
        Magic
    }

    [CreateAssetMenu(menuName = "RPG/Items/Weapon")]
    public class Weapon : EquippableItem
    {
        [Header("Weapon related")]
        public string AnimationName;
        public WeaponType WeaponType;
        public bool IsProjectile;
    }
}