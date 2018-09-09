using RPG.Characters;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Core
{
    public class GameManager : Singleton<GameManager>
    {
        public delegate void OnExperienceAwarded();
        public event OnExperienceAwarded InvokeOnExperienceAwarded;

        [Header("Managers")]
        public UIManager uIManager;
        public KeybindManager keybindManager;
        public SaveGameManager savegameManager;
        public PlayerControl player;

        [Header("UI References")]
        public AlertMessageController alertMessageController;
        public Castbar castbar;
        public AbilityButtonPanel abilityButtonPanel;

        [Header("Master Lists")]
        [SerializeField] Ability[] masterAbilityList;
        [SerializeField] Guild[] masterGuildList;
        [SerializeField] AnimationClip[] baseCharacterAnimationClips; 

        HealthController playerHealthController;
        AbilityController playerAbilityController;
        const float HEALTH_REGEN_AMOUNT = 0.25f;
        const float ENERGY_REGEN_AMOUNT = 0.5f;
        int playerExperience;

        public Ability[] MasterAbilityList { get { return masterAbilityList; } }
        public Guild[] MasterGuildList { get { return masterGuildList; } }
        public AnimationClip[] BaseCharacterAnimationClips { get { return baseCharacterAnimationClips; } }
        public int PlayerExperience
        {
            get { return playerExperience; }
            set
            {
                playerExperience = value;
                InvokeExperienceEvent();
                savegameManager.PlayerXP = playerExperience;
            }
        }

        void Start()
        {
            LoadExperienceAmount();
            playerHealthController = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
            playerAbilityController = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityController>();
        }

        void Update()
        { 
            RegenHealth(HEALTH_REGEN_AMOUNT);
            RegenEnergy(ENERGY_REGEN_AMOUNT);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        void RegenHealth(float value)
        {
            playerHealthController.CurrentHealthPoints += value * Time.deltaTime;
        }

        void RegenEnergy(float value)
        {
            playerAbilityController.CurrentEnergyPoints += value * Time.deltaTime;
        }

        void InvokeExperienceEvent()
        {
            InvokeOnExperienceAwarded += uIManager.UpdateExperienceText;
            InvokeOnExperienceAwarded();
            InvokeOnExperienceAwarded -= uIManager.UpdateExperienceText;
        }

        void LoadExperienceAmount()
        {
            if (savegameManager.PlayerXP > 0)
            {
                playerExperience = savegameManager.PlayerXP;
                uIManager.UpdateExperienceText();
            }
            else
            {
                playerExperience = 0;
                uIManager.UpdateExperienceText();
            }
        }
    }
}