using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class FireballUI : AbilityUI
    {
        AbilityStat attackSpeed;
        AbilityStat damage;
        AbilityStat critChance;
        AbilityStat critEffect;

        List<AbilityStat> abilityStats = new List<AbilityStat>();

        public override StatParams ReturnParams()
        {
            attackSpeed = ability.AttackSpeed;
            damage = (ability as FireballConfig).Damage;
            critChance = (ability as FireballConfig).CritChance;
            critEffect = (ability as FireballConfig).CritEffect;

            abilityStats.Add(attackSpeed);
            abilityStats.Add(damage);
            abilityStats.Add(critChance);
            abilityStats.Add(critEffect);

            StatParams statParams = new StatParams(abilityStats);

            return statParams;
        }
    }
}