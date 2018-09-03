using System.Text;
using UnityEngine;
using TMPro;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI itemNameText;
        [SerializeField] TextMeshProUGUI itemSlotText;
        [SerializeField] TextMeshProUGUI itemStatsText;

        StringBuilder sb = new StringBuilder();

        public void ShowTooltip(EquippableItem item)
        {
            itemNameText.text = item.name;
            itemSlotText.text = item.EquipmentType.ToString();

            sb.Length = 0;
            AddStat(item.ConstitutionBonus, "Constitution");
            AddStat(item.DexterityBonus, "Dexterity");
            AddStat(item.StrengthBonus, "Strength");
            AddStat(item.WisdomBonus, "Wisdom");
            AddStat(item.ArmourBonus, "Armour");

            itemStatsText.text = sb.ToString();

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        void AddStat(float value, string statName)
        {
            if (value != 0)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                if (value > 0)
                    sb.Append("+");

                sb.Append(value);
                sb.Append(" ");
                sb.Append(statName);
            }
        }
    }
}