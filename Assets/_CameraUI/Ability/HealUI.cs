using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class HealUI : AbilityUI
    {
        AbilityStat abilitySpeed;
        AbilityStat cooldown;
        AbilityStat energy;
        AbilityStat healAmount;
        AbilityStat critChance;
        AbilityStat critEffect;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override List<AbilityStat> ReturnStats()
        {
            abilitySpeed = ability.AbilitySpeed;
            cooldown = ability.Cooldown;
            energy = ability.Energy;
            healAmount = (ability as HealConfig).HealAmount;
            critChance = (ability as HealConfig).CritChance;
            critEffect = (ability as HealConfig).CritEffect;

            abilityStats.Add(abilitySpeed);
            abilityStats.Add(cooldown);
            abilityStats.Add(energy);
            abilityStats.Add(healAmount);
            abilityStats.Add(critChance);
            abilityStats.Add(critEffect);

            return abilityStats;
        }
    }
}