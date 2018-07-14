using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [TextArea(5, 5)] [SerializeField] string[] dialogueLines;
        [TextArea(2, 2)] [SerializeField] string[] acceptButtonLines;

        public string[] DialogueLines { get { return dialogueLines; } }
        public string[] AcceptButtonLines { get { return acceptButtonLines; } }
    }
}