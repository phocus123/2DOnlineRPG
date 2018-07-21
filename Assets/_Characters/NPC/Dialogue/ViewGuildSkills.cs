﻿using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/Events/View Guild Skills")]
    public class ViewGuildSkills : DialogueEvent
    {
        public override void PerformEventAction(NPCControl npc)
        {
            string characterName = npc.GetComponent<Character>().CharacterName;
            Guild[] guildList = FindObjectOfType<GameManager>().MasterGuildList;
            Guild guildToOpen = Array.Find(guildList, x => x.GuildLeader.CharacterName == characterName);

            FindObjectOfType<UIManager>().ShowGuildAbilities(guildToOpen);
        }

        public override bool QueryEvent()
        {
            return true;
        }
    }
}