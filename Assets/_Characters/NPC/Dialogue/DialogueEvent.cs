using UnityEngine;

namespace RPG.Characters
{
    public abstract class DialogueEvent : ScriptableObject
    {
        public abstract void PerformEventAction(NPCControl npc);
        public abstract bool QueryEvent();
    }
}