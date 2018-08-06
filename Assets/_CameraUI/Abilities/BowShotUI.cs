using System.Collections.Generic;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class BowShotUI : AbilityUI
    {
        AbilityStat attackSpeed;
        AbilityStat damage;
        AbilityStat critChance;
        AbilityStat critEffect;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override List<AbilityStat> ReturnStats()
        {
            attackSpeed = ability.AttackSpeed;
            damage = (ability as BowShotConfig).Damage;
            critChance = (ability as BowShotConfig).CritChance;
            critEffect = (ability as BowShotConfig).CritEffect;

            abilityStats.Add(attackSpeed);
            abilityStats.Add(damage);
            abilityStats.Add(critChance);
            abilityStats.Add(critEffect);

            return abilityStats;
        }
    }
}