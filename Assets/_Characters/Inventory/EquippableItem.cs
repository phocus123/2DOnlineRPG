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
    [CreateAssetMenu(menuName = "RPG/Equippable Item")]
    public class EquippableItem : Item
    {
        [Space]
        public int StrengthBonus;
        public int DexterityBonue;
        public int IntellectBonus;
        public int WisdomBonus;
        public int ConstitutionBonus;
        [Space]
        public EquipmentType EquipmentType;

        public void Equip(CharacterStats characterStats)
        {
            if (StrengthBonus != 0)
            {
                characterStats.PrimaryStats[3].AddModifier(new StatModifier(StrengthBonus, this));
            }
        }

        public void UnEquip(CharacterStats characterStats)
        {
            characterStats.PrimaryStats[3].RemoveAllModifiersFromSource(this);
        }
    }
}