using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Dialogue/Events/Guild Invite")]
    public class GuildInvite : DialogueEvent
    {
        SaveGameManager savegameManager;

        public override void PerformEventAction(NPCControl npc)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            Character npcCharacter = npc.GetComponent<Character>();
            Guild guildToJoin = Array.Find(gameManager.MasterGuildList, x => x.GuildLeader.CharacterName == npcCharacter.CharacterName);

            InviteToGuild(guildToJoin);
        }

        public override bool QueryEvent()
        {
            savegameManager = FindObjectOfType<SaveGameManager>();
            return savegameManager.HasGuild; // return true if player does not have a guild.
        }

        void InviteToGuild(Guild guild)
        {
            savegameManager.PlayerGuild = guild;
            savegameManager.HasGuild = true;
        }
    }
}