using System;

namespace RPG.Characters
{
    [Serializable]
    public class DialogueButtons
    {
        [Serializable]
        public class ButtonRow
        {
            public string optionText;
            public DialogueEvent dialogueEvent;
        }

        public ButtonRow[] buttonOptions;

        public string this[int rowIndex]
        {
            get { return buttonOptions[rowIndex].optionText; }
        }
    }
}