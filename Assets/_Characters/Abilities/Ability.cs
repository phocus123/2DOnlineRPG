using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class Ability : ScriptableObject, IMoveable
    {
        [Header("General Ability Data")]
        [TextArea(3, 5)] [SerializeField] string description;
        [SerializeField] Sprite icon;
        [SerializeField] Weapon weapon;
        [SerializeField] Guild guild;

        [Header("General Ability Stats")]
        [SerializeField] AbilityStat attackSpeed;
        [SerializeField] AbilityStat cooldown;
        [SerializeField] AbilityStat energy;

        [Header("Experience Cost")]
        [SerializeField] private int currentExperienceCost;
        [SerializeField] int experienceMultiplier;
        [SerializeField] int level;

        AbilityBehaviour behaviour;
        AbilityUI abilityUI;

        public AbilityBehaviour Behaviour { get { return behaviour; } }
        public AbilityUI AbilityUI { get { return abilityUI; } }
        public AbilityStat AttackSpeed { get { return attackSpeed; } }
        public float Cooldown {get { return cooldown.Value; } }
        public string Description { get { return description; } }
        public Sprite Icon { get { return icon; } }
        public Weapon Weapon { get { return weapon; } }
        public Guild Guild { get { return guild; } }
        public AbilityStat Energy { get { return energy; } }
        public int ExperienceMultiplier { get { return experienceMultiplier; } }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int CurrentExperienceCost
        {
            get { return currentExperienceCost; }
            set { currentExperienceCost = value; }
        }

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);
        public abstract AbilityUI GetUIComponent(GameObject objectToAttachTo);

        public void Use(GameObject target)
        {
            behaviour.Use(target);
        }

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(gameObjectToAttachTo);
            behaviourComponent.Ability = this;
            behaviour = behaviourComponent;
        }

        public void AttachAbilityUITo(GameObject gameObjectToAttachTo)
        {
            AbilityUI abilityUI = GetUIComponent(gameObjectToAttachTo);
            abilityUI.Ability = this;
            this.abilityUI = abilityUI;
            this.abilityUI.GetStats();
        }
    }
}
