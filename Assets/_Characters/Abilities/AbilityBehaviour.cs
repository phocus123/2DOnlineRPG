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
        public CharacterStat reliantStat;
        public float statMultiplier;

        public AbilityUseParams(GameObject target, float baseDamage, GameObject projectilePrefab, Ability ability, CharacterStat reliantStat, float statMultiplier)
        {
            this.target = target;
            this.baseDamage = baseDamage;
            this.projectilePrefab = projectilePrefab;
            this.ability = ability;
            this.reliantStat = reliantStat;
            this.statMultiplier = statMultiplier;
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