using RPG.CameraUI;
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
        [SerializeField] float abilityRange;
        [SerializeField] int level;

        [Header("General Ability Stats")]
        [SerializeField] AbilityStat abilitySpeed;
        [SerializeField] AbilityStat cooldown;
        [SerializeField] AbilityStat energy;
        [SerializeField] bool cooldownActive = false;

        AbilityBehaviour behaviour;
        AbilityUI abilityUI;

        public float AbilityRange { get { return abilityRange; } }
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public AbilityStat AbilitySpeed { get { return abilitySpeed; } }
        public AbilityStat Cooldown { get { return cooldown; } }
        public AbilityStat Energy { get { return energy; } }
        public bool CooldownActive
        {
            get { return cooldownActive; }
            set { cooldownActive = value; }
        }

        public AbilityBehaviour Behaviour { get { return behaviour; } }
        public AbilityUI AbilityUI { get { return abilityUI; } }
        public Sprite Icon { get { return icon; } }

        protected virtual void OnValidate()
        {
            abilitySpeed.statName = "Ability Speed";
            cooldown.statName = "Cooldown";
            energy.statName = "Energy";
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

    public struct AbilityUseParams
    {
        public GameObject target;
        public float baseDamage;
        public GameObject projectilePrefab;
        public Ability ability;
        public CharacterStat reliantStat;
        public float statMultiplier;
        public string hitAnimationName;

        public AbilityUseParams(GameObject target, float baseDamage, GameObject projectilePrefab, Ability ability, CharacterStat reliantStat, float statMultiplier, string hitAnimationName)
        {
            this.target = target;
            this.baseDamage = baseDamage;
            this.projectilePrefab = projectilePrefab;
            this.ability = ability;
            this.reliantStat = reliantStat;
            this.statMultiplier = statMultiplier;
            this.hitAnimationName = hitAnimationName;
        }
    }
}
