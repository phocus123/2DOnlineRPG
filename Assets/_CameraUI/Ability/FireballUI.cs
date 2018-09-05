using System.Collections.Generic;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class FireballUI : AbilityUI
    {
        AbilityStat attackSpeed;
        AbilityStat cooldown;
        AbilityStat energy;
        AbilityStat damage;
        AbilityStat critChance;
        AbilityStat critEffect;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override List<AbilityStat> ReturnStats()
        {
            attackSpeed = ability.AbilitySpeed;
            cooldown = ability.Cooldown;
            energy = ability.Energy;
            damage = (ability as FireballConfig).Damage;
            critChance = (ability as FireballConfig).CritChance;
            critEffect = (ability as FireballConfig).CritEffect;

            abilityStats.Add(attackSpeed);
            abilityStats.Add(cooldown);
            abilityStats.Add(energy);
            abilityStats.Add(damage);
            abilityStats.Add(critChance);
            abilityStats.Add(critEffect);

            return abilityStats;
        }
    }
}