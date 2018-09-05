using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class DaggerSlashUI : AbilityUI
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
            damage = (ability as DaggerSlashConfig).Damage;
            critChance = (ability as DaggerSlashConfig).CritChance;
            critEffect = (ability as DaggerSlashConfig).CritEffect;

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