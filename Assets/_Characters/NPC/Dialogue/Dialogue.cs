using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/NPC Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [TextArea(3,3)][SerializeField] string npcIntroduction;
        [SerializeField] DialogueLines dialogueLines;
        [SerializeField] DialogueButtons dialogueButtons;
        
        public string NPCIntroduction { get { return npcIntroduction; } }
        public DialogueLines DialogueLines { get { return dialogueLines; } }
        public DialogueButtons DialogueButtons { get { return dialogueButtons; } }
    }
}