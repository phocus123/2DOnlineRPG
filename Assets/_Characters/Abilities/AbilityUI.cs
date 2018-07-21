using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public struct StatParams 
    {
        public List<AbilityStat> stats;

        public StatParams(List<AbilityStat> stats)
        {
            this.stats = stats;
        }

        public List<AbilityStat> Stats { get { return stats; } }
    }

    public abstract class AbilityUI : MonoBehaviour
    {
        protected Ability ability;

        StatParams statParams;

        public StatParams StatParams { get { return statParams; } }

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        public void SetParams()
        {
            statParams = ReturnParams();
        }

        public abstract StatParams ReturnParams();
    }
}