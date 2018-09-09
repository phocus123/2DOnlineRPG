using UnityEngine;
using TMPro;
using RPG.Characters;
using System.Text;

namespace RPG.CameraUI
{
    public class StatTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI statNameText;
        [SerializeField] TextMeshProUGUI statModifiersText;

        StringBuilder sb = new StringBuilder();

        public void ShowTooltip(CharacterStat stat, string statName)
        {
            statNameText.text = GetStatTopText(stat, statName);
            statModifiersText.text = GetStatModifiersText(stat);

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        string GetStatTopText(CharacterStat stat, string statName)
        {
            sb.Length = 0;
            sb.Append(statName);
            sb.Append(" ");
            sb.Append(stat.Value);
           
            if (stat.Value != stat.BaseValue)
            {
                sb.Append(" (");
                sb.Append(stat.BaseValue);

                if (stat.Value > stat.BaseValue)
                    sb.Append("+");

                sb.Append(System.Math.Round(stat.Value - stat.BaseValue, 4));
                sb.Append(" ) ");
            }

            return sb.ToString();
        }

        string GetStatModifiersText(CharacterStat stat)
        {
            sb.Length = 0;

            foreach (StatModifier modifier in stat.StatModifiers)
            {
                if (sb.Length > 0)
                    sb.AppendLine();

                if (modifier.Value > 0)
                    sb.Append("+");

                sb.Append(modifier.Value);

                EquippableItem item = modifier.Source as EquippableItem;

                if (item != null)
                {
                    sb.Append(" ");
                    sb.Append(item.name);
                }
                else
                {
                    Debug.LogError("Modifier is not an equippable item.");
                }
            }

            return sb.ToString();
        }
    }
}