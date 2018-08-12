using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        public delegate void OnHitTarget(AbilityUseParams abilityUseParams);
        public event OnHitTarget InvokeOnHitTarget;

        public Transform Target { get; private set; }

        [SerializeField] private float speed = 0;

        Rigidbody2D rigidBody;
        AbilityUseParams abilityUseParams;

        const string ARROW_IMPACT = "ArrowImpact";
        const string FIREBALL_IMPACT = "FireballImpact";

        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (Target != null)
            {
                Vector2 direction = Target.transform.position - transform.position;
                rigidBody.velocity = direction.normalized * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        public void Initialize(Transform target, AbilityUseParams abilityUseParams)
        {
            Target = target;
            this.abilityUseParams = abilityUseParams;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Hitbox" && collision.transform.parent == Target)
            {
                if (abilityUseParams.ability.Weapon.name == "Bow")
                {
                    GetComponent<Animator>().SetTrigger(ARROW_IMPACT);
                }
                else if (abilityUseParams.ability.Weapon.name == "Magic")
                {
                    GetComponent<Animator>().SetTrigger(FIREBALL_IMPACT);
                }

                InvokeOnHitTarget(abilityUseParams);
                rigidBody.velocity = Vector2.zero;
                Target = null;
            }
        }
    }
}