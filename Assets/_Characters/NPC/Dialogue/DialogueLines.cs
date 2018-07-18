using System;
using UnityEngine;

namespace RPG.Characters
{
    [Serializable]
    public class DialogueLines
    {
        [Serializable]
        public class DialogueRow
        {
            [TextArea(5, 5)] public string[] dialogueLines;
        }

        public DialogueRow[] dialogueOptions;

        public string this[int rowIndex, int colIndex]
        {
            get { return dialogueOptions[rowIndex].dialogueLines[colIndex]; }
        }

        public string[] this[int rowindex]
        {
            get { return dialogueOptions[rowindex].dialogueLines; }
        }
    }
}