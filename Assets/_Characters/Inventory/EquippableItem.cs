using UnityEngine;

public enum EquipmentType
{
    Boots,
    Legs,
    Chest,
    Shoulders,
    Neck,
    Head,
    Bracers,
    Gloves,
    Ring1,
    Ring2,
    PrimaryWeapon,
    SecondaryWeapon
}

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Items/Equippable Item")]
    public class EquippableItem : Item
    {
        [Header("Stat Bonuses")]
        public int StrengthBonus;
        public int DexterityBonue;
        public int IntellectBonus;
        public int WisdomBonus;
        public int ConstitutionBonus;
        [Space]
        public EquipmentType EquipmentType;
        [Header("Equipment Animations")]
        [SerializeField] AnimationClip[] animationClips;

        public delegate void OnItemEquipped(EquipmentType equipmentType, EquippableItem item);
        public event OnItemEquipped ItemEquipped;
        public delegate void OnItemUnEquipped(EquipmentType equipmentType, EquippableItem item);
        public event OnItemEquipped ItemUnEquipped;

        public AnimationClip[] AnimationClips { get { return animationClips; } }

        public void Equip(CharacterStats characterStats)
        {
            // TODO Complete this and possible change to concrete implementations within character stats class rather than scriptable object.
            if (StrengthBonus != 0)
            {
                characterStats.PrimaryStats[3].AddModifier(new StatModifier(StrengthBonus, this));
            }

            ItemEquipped(EquipmentType, this);
        }

        public void UnEquip(CharacterStats characterStats)
        {
            characterStats.PrimaryStats[3].RemoveAllModifiersFromSource(this);
            ItemUnEquipped(EquipmentType, this);
        }
    }
}