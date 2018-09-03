using UnityEngine;
using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] StatDisplay[] StatDisplays;
        [SerializeField] string[] statNames;

        CharacterStat[] characterStats;

        private void OnValidate()
        {
            StatDisplays = GetComponentsInChildren<StatDisplay>();
            UpdateStatNames();
        }

        public void SetPrimaryStats(CharacterStat[] primaryStats)
        {
            this.characterStats = primaryStats;

            if (this.characterStats.Length > StatDisplays.Length)
            {
                Debug.LogError("Not enough stat displays."); 
                return;
            }

            for (int i = 0; i < StatDisplays.Length; i++)
            {
                StatDisplays[i].gameObject.SetActive(i < this.characterStats.Length);

                if (i < primaryStats.Length)
                {
                     StatDisplays[i].Stat = primaryStats[i];
                }
            }
        }

        public void UpdateStatValues()
        {
            for (int i = 0; i < characterStats.Length; i++)
            {
                StatDisplays[i].UpdateStatValue();
            }
        }

        public void UpdateStatNames()
        {
            if (statNames.Length > 0)
            {
                for (int i = 0; i < statNames.Length; i++)
                {
                    StatDisplays[i].NameText.text = statNames[i];
                }
            }
        }
    }
}