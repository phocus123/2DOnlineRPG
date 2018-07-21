using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/Events/Close UI Canvas")]
    public class CloseUICanvas : DialogueEvent
    {
        public override void PerformEventAction(NPCControl npc)
        {
            var uiManager = FindObjectOfType<UIManager>();

            uiManager.DialogueUI.CloseDialogue();
            uiManager.GuildAbilityUI.CloseGuildAbilities();
        }

        public override bool QueryEvent()
        {
            return true;
        }

    }
}