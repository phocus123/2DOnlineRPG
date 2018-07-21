using System;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class Ability : ScriptableObject, IMoveable
    {
        [Header("General Ability Data")]
        [SerializeField] private AbilityStat attackSpeed;
        [SerializeField] private AbilityStat cooldown = null;
        [SerializeField] private float energy;
        [TextArea(3, 5)] [SerializeField] private string description = string.Empty;
        [SerializeField] private Sprite icon = null;
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private Guild guild;

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
        public float Energy { get { return energy; } }

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
            this.abilityUI.SetParams();
        }
    }
}
