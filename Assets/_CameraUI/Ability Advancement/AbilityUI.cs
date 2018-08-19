using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class AbilityUI : MonoBehaviour
    {
        protected Ability ability;
        protected List<AbilityStat> stats;

        public List<AbilityStat> Stats { get { return stats; } }

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        public void GetStats()
        {
            stats = ReturnStats();
        }

        public abstract List<AbilityStat> ReturnStats();
    }
}