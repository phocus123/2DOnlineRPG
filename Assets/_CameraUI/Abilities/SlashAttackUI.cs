using RPG.Characters;
using System.Collections.Generic;

namespace RPG.CameraUI
{
    public class SlashAttackUI : AbilityUI
    {
        AbilityStat attackSpeed;
        AbilityStat damage;
        AbilityStat critChance;
        AbilityStat critEffect;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override List<AbilityStat> ReturnStats()
        {
            attackSpeed = ability.AttackSpeed;
            damage = (ability as SlashAttackConfig).Damage;
            critChance = (ability as SlashAttackConfig).CritChance;
            critEffect = (ability as SlashAttackConfig).CritEffect;

            abilityStats.Add(attackSpeed);
            abilityStats.Add(damage);
            abilityStats.Add(critChance);
            abilityStats.Add(critEffect);

            return abilityStats;
        }
    }
}