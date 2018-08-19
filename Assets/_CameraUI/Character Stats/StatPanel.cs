using UnityEngine;
using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] StatDisplay[] statDisplays;
        [SerializeField] string[] statNames;

        PrimaryStat[] stats;

        private void OnValidate()
        {
            statDisplays = GetComponentsInChildren<StatDisplay>();
            UpdateStatNames();  
        }

        public void SetStats(PrimaryStat[] primaryStats)
        {
            stats = primaryStats;

            if (stats.Length > statDisplays.Length)
            {
                Debug.LogError("Not enough stat displays.");
                return;
            }

            for (int i = 0; i < statDisplays.Length; i++)
            {
                statDisplays[i].gameObject.SetActive(i < stats.Length);
            }
        }

        public void UpdateStatValues()
        {
            for (int i = 0; i < stats.Length; i++)
            {
                statDisplays[i].ValueText.text = stats[i].Value.ToString();
            }
        }

        public void UpdateStatNames()
        {
            for (int i = 0; i < statNames.Length; i++)
            {
                statDisplays[i].NameText.text = statNames[i];
            }
        }
    }
}