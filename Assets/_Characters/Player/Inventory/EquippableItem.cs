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

        public void Equip(Character characterStats)
        {
            if (ConstitutionBonus != 0)
            {
                characterStats.CharacterrStats[0].AddModifier(new StatModifier(ConstitutionBonus, this));
            }
            if (DexterityBonus != 0)
            {
                characterStats.CharacterrStats[1].AddModifier(new StatModifier(DexterityBonus, this));
            }
            if (IntellectBonus != 0)
            {
                characterStats.CharacterrStats[2].AddModifier(new StatModifier(IntellectBonus, this));
            }
            if (StrengthBonus != 0)
            {
                characterStats.CharacterrStats[3].AddModifier(new StatModifier(StrengthBonus, this));
            }
            if (WisdomBonus != 0)
            {
                characterStats.CharacterrStats[4].AddModifier(new StatModifier(WisdomBonus, this));
            }
            if (ArmourBonus != 0)
            {
                characterStats.CharacterrStats[5].AddModifier(new StatModifier(ArmourBonus, this));
            }

            ItemEquipped(EquipmentType, this);
        }

        public void UnEquip(Character characterStats)
        {
            characterStats.CharacterrStats[0].RemoveAllModifiersFromSource(this);
            characterStats.CharacterrStats[1].RemoveAllModifiersFromSource(this);
            characterStats.CharacterrStats[2].RemoveAllModifiersFromSource(this);
            characterStats.CharacterrStats[3].RemoveAllModifiersFromSource(this);
            characterStats.CharacterrStats[4].RemoveAllModifiersFromSource(this);
            characterStats.CharacterrStats[5].RemoveAllModifiersFromSource(this);

            ItemUnEquipped(EquipmentType, this);
        }

        
    }
}