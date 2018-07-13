using System.Collections.Generic;
using System;

[Serializable]
public class PlayerData
{
    private static PlayerData instance;
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }

    public Dictionary<int, string> AbilityDict { get; set; }
    public int PlayerExperience { get; set; }
}
