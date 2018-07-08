using UnityEngine;

namespace RPG.Characters
{
    public class SlashAttackBehaviour : AbilityBehaviour
    {
        private Coroutine attackRoutine;

        public override void Use(GameObject target)
        {
            //attackRoutine = StartCoroutine(Attack(useParams));
        }

        //private IEnumerator Attack(AbilityUseParams useParams)
        //{
        //    var damage = (config as SlashAttackConfig).Damage;
        //    var source = useParams.source;

        //    if (useParams.target.GetType() == typeof(Enemy))
        //    {
        //        var target = (Enemy)useParams.target;
        //        target.transform.GetChild(0).GetComponent<Animator>().SetTrigger("MeleeImpact");
        //        target.TakeDamage(damage.TrueValue, gameObject.transform);
        //    }
        //    else
        //    {
        //        var target = (Player)useParams.target;
        //        target.transform.GetChild(0).GetComponent<Animator>().SetTrigger("MeleeImpact");
        //        target.TakeDamage(damage.TrueValue, gameObject.transform);
        //    }

        //    source.StartAttack(useParams.ability);
        //    source.Energy.CurrentValue -= config.Energy;


        //    yield return new WaitForSeconds(config.AttackSpeed);
        //    StopAttack(useParams);

        //}

        //private void StopAttack(AbilityUseParams useParams)
        //{
        //    if (attackRoutine != null)
        //    {
        //        StopCoroutine(attackRoutine);
        //        useParams.source.StopAttack(useParams.ability);
        //    }
        //}
    }
}