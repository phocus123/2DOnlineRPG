using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class Ability : ScriptableObject, IMoveable
    {
        [Header("General Ability Data")]
        [TextArea(3, 5)] public string Description;
        public Sprite icon;
        public WeaponType WeaponType;
        public string AnimationName;
        public Guild Guild;
        [SerializeField] float attackRange;
        [SerializeField] int level;

        [Header("General Ability Stats")]
        [SerializeField] AbilityStat attackSpeed;
        [SerializeField] AbilityStat cooldown;
        [SerializeField] AbilityStat energy;

        AbilityBehaviour behaviour;
        AbilityUI abilityUI;

        public float AttackRange { get { return attackRange; } }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public AbilityStat AttackSpeed { get { return attackSpeed; } }
        public AbilityStat Cooldown { get { return cooldown; } }
        public AbilityStat Energy { get { return energy; } }

        public AbilityBehaviour Behaviour { get { return behaviour; } }
        public AbilityUI AbilityUI { get { return abilityUI; } }
        public Sprite Icon { get { return icon; } }
         
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
            behaviourComponent.Character = gameObjectToAttachTo.GetComponent<Character>();
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
