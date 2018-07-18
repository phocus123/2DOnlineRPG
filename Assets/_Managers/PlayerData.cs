using System.Collections.Generic;
using System;
using RPG.Characters;
using UnityEngine;

namespace RPG.Core
{
    [Serializable]
    public class PlayerData 
    {
        public Dictionary<int, string> AbilityDict { get; set; }
        public int PlayerExperience { get; set; }
        public bool HasGuild { get; set; }
        public int GuildInstanceId { get; set; }
    }
}