using UnityEngine;
using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class StatPanel : MonoBehaviour
    {
        [Header("Primary Stats")]
        [SerializeField] StatDisplay[] primaryStatDisplays;
        [SerializeField] string[] primaryStatNames;
        [Header("Secondary Stats")]
        [SerializeField] StatDisplay[] secondaryStatDisplays;
        [SerializeField] string[] secondaryStatNames;

        PrimaryStat[] primaryStats;
        SecondaryStat[] secondaryStats;

        private void OnValidate()
        {
            UpdateStatNames();
        }

        public void SetPrimaryStats(PrimaryStat[] primaryStats)
        {
            this.primaryStats = primaryStats;

            if (this.primaryStats.Length > primaryStatDisplays.Length)
            {
                Debug.LogError("Not enough stat displays.");
                return;
            }

            for (int i = 0; i < primaryStatDisplays.Length; i++)
            {
                primaryStatDisplays[i].gameObject.SetActive(i < this.primaryStats.Length);
            }
        }

        public void SetSecondaryStats(params SecondaryStat[] secondaryStats)
        {
            this.secondaryStats = secondaryStats;

            if (this.secondaryStats.Length > secondaryStatDisplays.Length)
            {
                Debug.LogError("Not enough stat displays.");
                return;
            }

            for (int i = 0; i < secondaryStatDisplays.Length; i++)
            {
                secondaryStatDisplays[i].gameObject.SetActive(i < this.secondaryStats.Length);
            }
        }

        public void UpdateStatValues()
        {
            for (int i = 0; i < primaryStats.Length; i++)
            {
                primaryStatDisplays[i].ValueText.text = primaryStats[i].Value.ToString();
            }

            for (int i = 0; i < secondaryStats.Length; i++)
            {
                secondaryStatDisplays[i].ValueText.text = secondaryStats[i].Value.ToString();
            }
        }

        public void UpdateStatNames()
        {
            if (primaryStatNames.Length > 0)
            {
                for (int i = 0; i < primaryStatNames.Length; i++)
                {
                    primaryStatDisplays[i].NameText.text = primaryStatNames[i];
                }
            }

            if (secondaryStatNames.Length > 0)
            {
                for (int i = 0; i < secondaryStatNames.Length; i++)
                {
                    secondaryStatDisplays[i].NameText.text = secondaryStatNames[i];
                }
            }
        }
    }
}