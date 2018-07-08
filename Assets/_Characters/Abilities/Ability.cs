using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public abstract class Ability : ScriptableObject, IMoveable
    {
        [Header("General Ability Data")]

        [SerializeField] private AbilityStat attackSpeed;
        public float AttackSpeed
        {
            get { return attackSpeed.Value; }
        }

        [SerializeField] private AbilityStat cooldown = null;
        public float Cooldown
        {
            get { return cooldown.Value; }
        }

        [SerializeField] private float energy;
        public float Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        [TextArea(3, 5)] [SerializeField] private string description = string.Empty;
        public string Description
        {
            get { return description; }
        }


        [SerializeField] private Sprite icon = null;
        public Sprite Icon
        {
            get { return icon; }
        }

        [SerializeField] private Weapon weapon = null;
        public Weapon Weapon
        {
            get { return weapon; }
        }

        [SerializeField] private Material guildMaterial;
        public Material GuildMaterial
        {
            get { return guildMaterial; }
            set { guildMaterial = value; }
        }

        protected AbilityBehaviour behaviour;

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
