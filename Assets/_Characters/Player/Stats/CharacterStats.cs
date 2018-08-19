using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class CharacterStats : MonoBehaviour
    {
        [SerializeField] PrimaryStat[] primaryStats;
        [SerializeField] StatPanel statPanel;

        public PrimaryStat[] PrimaryStats { get { return primaryStats; } }
        public StatPanel StatPanel { get { return statPanel; } }

        private void Awake()
        {
            statPanel.SetStats(primaryStats);
            statPanel.UpdateStatValues();
        }
    }
}