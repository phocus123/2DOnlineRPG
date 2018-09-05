using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class UseableItemEffect : ScriptableObject
    {
        public abstract void ExecuteEffect(UseableItem parentItem, Character character);
    }
}