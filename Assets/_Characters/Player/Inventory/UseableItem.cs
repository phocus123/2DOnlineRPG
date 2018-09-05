using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName ="RPG/Items/Useable Item")]
    public class UseableItem : Item
    {
        public bool IsConsumable;
        public List<UseableItemEffect> UseableItemEffects;

        public virtual void Use(Character character)
        {
            foreach (UseableItemEffect effect in UseableItemEffects)
            {
                effect.ExecuteEffect(this, character);
            }
        }
    }
}