using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/Events/View Guild Skills")]
    public class ViewGuildSkills : DialogueEvent
    {
        public override void PerformEventAction(NPCControl npc)
        {
            Debug.Log("show guild skills");
        }

        public override bool QueryEvent()
        {
            throw new System.NotImplementedException();
        }
    }
}