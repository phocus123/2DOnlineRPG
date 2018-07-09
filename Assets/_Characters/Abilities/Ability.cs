using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public abstract class Ability : ScriptableObject, IMoveable
    {
        [Header("General Ability Data")]
        [SerializeField] private AbilityStat attackSpeed;
        [SerializeField] private AbilityStat cooldown = null;
        [SerializeField] private float energy;
        [TextArea(3, 5)] [SerializeField] private string description = string.Empty;
        [SerializeField] private Sprite icon = null;
        [SerializeField] private Weapon weapon = null;
        [SerializeField] private Material guildMaterial;

        protected AbilityBehaviour behaviour;

        public float AttackSpeed
        {
            get { return attackSpeed.Value; }
        }

        public float Cooldown
        {
            get { return cooldown.Value; }
        }

        public float Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        public string Description
        {
            get { return description; }
        }

        public Sprite Icon
        {
            get { return icon; }
        }

        public Weapon Weapon
        {
            get { return weapon; }
        }

        public Material GuildMaterial
        {
            get { return guildMaterial; }
            set { guildMaterial = value; }
        }

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

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
    }
}
