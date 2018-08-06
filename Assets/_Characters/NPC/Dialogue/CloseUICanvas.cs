﻿using UnityEngine;
using RPG.Core;
using RPG.CameraUI;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/Events/Close UI Canvas")]
    public class CloseUICanvas : DialogueEvent
    {
        public override void PerformEventAction(NPCControl npc)
        {
            var abilityButtonPanel = FindObjectOfType<AbilityButtonPanel>();
            var uiManager = FindObjectOfType<UIManager>();

            uiManager.DialogueUI.CloseDialogue();
            abilityButtonPanel.CloseGuildAbilitiesCanvas();
        }

        public override bool QueryEvent()
        {
            return true;
        }

    }
}