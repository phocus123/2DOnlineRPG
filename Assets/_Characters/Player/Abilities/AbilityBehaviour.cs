using System.Collections;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public GameObject target;
        public float baseDamage;
        public GameObject projectilePrefab;
        public Ability ability;
        public PrimaryStat reliantStat;
        public float statMultiplier;
        public string animationTrigger;

        public AbilityUseParams(GameObject target, float baseDamage, GameObject projectilePrefab, Ability ability, PrimaryStat reliantStat, float statMultiplier, string animationTrigger)
        {
            this.target = target;
            this.baseDamage = baseDamage;
            this.projectilePrefab = projectilePrefab;
            this.ability = ability;
            this.reliantStat = reliantStat;
            this.statMultiplier = statMultiplier;
            this.animationTrigger = animationTrigger;
        }
    }

    public abstract class AbilityBehaviour : MonoBehaviour
    {
        protected Ability ability;

        Character character;

        public abstract void Use(GameObject target);

        public Ability Ability
        {
            get { return ability; }
            set { ability = value; }
        }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }
    }
}