using RPG.Characters;
using UnityEngine;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnExperienceAwarded();
        public event OnExperienceAwarded InvokeOnExperienceAwarded;

        [Header("Managers")]
        public UIManager uiManager;
        public KeybindManager keybindManager;
        public SaveGameManager savegameManager;

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
            InvokeOnExperienceAwarded += uiManager.UpdateExperienceText;
            InvokeOnExperienceAwarded();
            InvokeOnExperienceAwarded -= uiManager.UpdateExperienceText;
        }

        void LoadExperienceAmount()
        {
            if (savegameManager.PlayerXP > 0)
            {
                playerExperience = savegameManager.PlayerXP;
                uiManager.UpdateExperienceText();
            }
            else
            {
                playerExperience = 0;
                uiManager.UpdateExperienceText();
            }
        }
    }
}