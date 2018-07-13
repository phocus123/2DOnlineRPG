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

        [Header("Abilities")]
        [SerializeField] Ability[] masterAbilityList;

        HealthSystem playerHealthSystem;
        AbilitySystem playerAbilitySystem;
        const float HEALTH_REGEN_AMOUNT = 0.25f;
        const float ENERGY_REGEN_AMOUNT = 0.5f;
        private int playerExperience;

        public Ability[] MasterAbilityList { get { return masterAbilityList; } }
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
            playerHealthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            playerAbilitySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySystem>();
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

        // TODO Move to PlayerControl.
        void TriggerDialogue(NPCControl npc)
        {
            //if (Input.GetMouseButtonDown(1))
            //{
            //    if (npc.TalkRange)
            //    {
            //        DialogueManager.Instance.CurrentNpc = npc;
            //        DialogueManager.Instance.OpenDialogue(player);
            //    }
            //}
        }

        void RegenHealth(float value)
        {
            playerHealthSystem.CurrentHealthPoints += value * Time.deltaTime;
        }

        void RegenEnergy(float value)
        {
            playerAbilitySystem.CurrentEnergyPoints += value * Time.deltaTime;
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