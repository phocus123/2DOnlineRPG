using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        HealthSystem playerHealthSystem;
        const float HEALTH_REGEN_AMOUNT = 0.25f;

        private void Start()
        {
            playerHealthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        }

        private void Update()
        { 
            RegenHealth(HEALTH_REGEN_AMOUNT);
            //RegenEnergy(1);
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

        //private void RegenEnergy(float value)
        //{
        //    player.Energy.CurrentValue += value * Time.deltaTime;
        //}
    }
}