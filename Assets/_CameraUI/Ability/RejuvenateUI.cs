using System.Collections.Generic;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class RejuvenateUI : AbilityUI
    {
        AbilityStat abilitySpeed;
        AbilityStat cooldown;
        AbilityStat energy;
        AbilityStat healAmount;
        AbilityStat healFrequency;
        AbilityStat duration;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override List<AbilityStat> ReturnStats()
        {
            abilitySpeed = ability.AbilitySpeed;
            cooldown = ability.Cooldown;
            energy = ability.Energy;
            healAmount = (ability as RejuvenationConfig).HealAmount;
            healFrequency = (ability as RejuvenationConfig).HealFrequency;
            duration = (ability as RejuvenationConfig).Duration;

            abilityStats.Add(abilitySpeed);
            abilityStats.Add(cooldown);
            abilityStats.Add(energy);
            abilityStats.Add(healAmount);
            abilityStats.Add(healFrequency);
            abilityStats.Add(duration);

            return abilityStats;
        }
    }
}