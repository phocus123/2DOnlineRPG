using UnityEngine;
using System;

namespace RPG.Characters
{
    [Serializable]
    [CreateAssetMenu(menuName = "RPG/Guild")]
    public class Guild : ScriptableObject
    {
        [SerializeField] Ability[] guildAbilities;
        [SerializeField] Character guildLeader;
        [SerializeField] Material guildMaterial;

        public Ability[] GuildAbilities { get { return guildAbilities; } }
        public Character GuildLeader { get { return guildLeader; } }
        public Material GuildMaterial { get { return guildMaterial; } }
    }
}