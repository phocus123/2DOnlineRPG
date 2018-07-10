using RPG.Characters;
using UnityEngine;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        public UIManager uiManager;
        public KeybindManager keybindManager;

        HealthSystem playerHealthSystem;
        AbilitySystem playerAbilitySystem;
        const float HEALTH_REGEN_AMOUNT = 0.25f;
        const float ENERGY_REGEN_AMOUNT = 0.5f;

        private void Start()
        {
            playerHealthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            playerAbilitySystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilitySystem>();
        }

        private void Update()
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
    }
}