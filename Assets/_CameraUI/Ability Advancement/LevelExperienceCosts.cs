using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG/Experience Costs")]
[Serializable]
public class LevelExperienceCosts : ScriptableObject
{
    public ExperienceCosts[] level;

    public int this[int rowIndex]
    {
        get { return level[rowIndex].experienceCost; }
    }

    [Serializable]
    public class ExperienceCosts
    {
        public int experienceCost;
    }
}
