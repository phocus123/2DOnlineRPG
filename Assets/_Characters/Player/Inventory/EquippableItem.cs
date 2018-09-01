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
        [Header("Primary Stat Bonuses")]
        public int ConstitutionBonus;
        public int DexterityBonus;
        public int IntellectBonus;
        public int StrengthBonus;
        public int WisdomBonus;
        [Header("Secondary Stat Bonuses")]
        public int ArmourBonus;
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
            if (ConstitutionBonus != 0)
            {
                characterStats.PrimaryStats[0].AddModifier(new StatModifier(ConstitutionBonus, this));
            }
            if (DexterityBonus != 0)
            {
                characterStats.PrimaryStats[1].AddModifier(new StatModifier(DexterityBonus, this));
            }
            if (IntellectBonus != 0)
            {
                characterStats.PrimaryStats[2].AddModifier(new StatModifier(IntellectBonus, this));
            }
            if (StrengthBonus != 0)
            {
                characterStats.PrimaryStats[3].AddModifier(new StatModifier(StrengthBonus, this));
            }
            if (WisdomBonus != 0)
            {
                characterStats.PrimaryStats[4].AddModifier(new StatModifier(WisdomBonus, this));
            }
            if (ArmourBonus != 0)
            {
                characterStats.Armour.AddModifier(new StatModifier(ArmourBonus, this));
            }

            ItemEquipped(EquipmentType, this);
        }

        public void UnEquip(CharacterStats characterStats)
        {
            characterStats.PrimaryStats[0].RemoveAllModifiersFromSource(this);
            characterStats.PrimaryStats[1].RemoveAllModifiersFromSource(this);
            characterStats.PrimaryStats[2].RemoveAllModifiersFromSource(this);
            characterStats.PrimaryStats[3].RemoveAllModifiersFromSource(this);
            characterStats.PrimaryStats[4].RemoveAllModifiersFromSource(this);

            characterStats.Armour.RemoveAllModifiersFromSource(this);

            ItemUnEquipped(EquipmentType, this);
        }

        
    }
}