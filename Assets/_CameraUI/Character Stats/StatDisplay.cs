using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isCharacterStat = false;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI ValueText;
        [SerializeField] StatTooltip statTooltip;

        CharacterStat stat;

        public CharacterStat Stat
        {
            get { return stat; }
            set
            {
                stat = value;
                UpdateStatValue();
            }
        }

        public void UpdateStatValue()
        {
            ValueText.text = stat.Value.ToString();
        }

        void OnValidate()
        {
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            NameText = texts[0];
            ValueText = texts[1];

            if (statTooltip == null)
                statTooltip = FindObjectOfType<StatTooltip>();
        }
         
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isCharacterStat)
                statTooltip.ShowTooltip(stat, stat.name);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isCharacterStat)
                statTooltip.HideTooltip();
        }
    }
} 