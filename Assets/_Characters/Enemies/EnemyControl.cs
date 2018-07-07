using UnityEngine;

namespace RPG.Characters
{
    public class EnemyControl : CharacterController
    {
        [SerializeField] GameObject target;

        Character character;
        Vector2 direction;

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        public override Vector2 GetDirection()
        {
            return direction;
        }

        public void Update()
        {
            direction = Vector2.zero;

            if (target != null)
            {
                direction = (target.transform.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, character.MoveSpeed * Time.deltaTime);

                //float distance = Vector2.Distance(target.position, parent.transform.position);

                //if (distance <= parent.EnemyAttackRange)
                //{
                //    parent.ChangeState(new AttackState());
                //}
            }
        }
    }
}