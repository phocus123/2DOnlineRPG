using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [Header("Player Stats")]
        [SerializeField] PrimaryStat[] primaryStats;
        [SerializeField] StatPanel statPanel;
        [Header("Character Stats")]
        public SecondaryStat Armour;

        public PrimaryStat[] PrimaryStats { get { return primaryStats; } }
        public StatPanel StatPanel { get { return statPanel; } }

        private void OnValidate()
        {
            Armour.AssociatedCharacter = GetComponent<Character>();
        }

        private void Awake()
        {
            if (StatPanel != null)
            {
                statPanel.SetPrimaryStats(primaryStats);
                StatPanel.SetSecondaryStats(Armour);
                statPanel.UpdateStatValues();
            }
        }
    }
}