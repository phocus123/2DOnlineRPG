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
        public float AttackRange;
        public Guild Guild;
        public int Level;

        [Header("General Ability Stats")]
        public AbilityStat AttackSpeed;
        public AbilityStat Cooldown;
        public AbilityStat Energy;

        AbilityBehaviour behaviour;
        AbilityUI abilityUI;

        public AbilityBehaviour Behaviour { get { return behaviour; } }
        public AbilityUI AbilityUI { get { return abilityUI; } }
        public Sprite Icon { get { return icon; } }

        void OnValidate()
        {
            AttackSpeed.statName = "Attack Speed";
            Cooldown.statName = "Cooldown";
            Energy.statName = "Energy"; 
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
