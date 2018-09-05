using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Items/Useable Item Effects/Heal Effect")]
    public class InstantHealEffect : UseableItemEffect
    {
        public int healAmount; 

        public override void ExecuteEffect(UseableItem parentItem, Character character)
        {
            character.GetComponent<HealthController>().CurrentHealthPoints += healAmount;
        }
    }
}