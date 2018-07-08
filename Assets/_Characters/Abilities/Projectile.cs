using UnityEngine;

namespace RPG.Characters
{
    public class Projectile : MonoBehaviour
    {
        public Transform Target { get; private set; }

        [SerializeField] private float speed = 0;

        Rigidbody2D rigidBody;
        Ability currentAbility;
        float damage;

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

        public void Initialize(Transform target, float damage, Ability ability = null)
        {
            Target = target;
            currentAbility = ability;
            this.damage = damage;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "ProjectileHitbox" && collision.transform.parent == Target)
            {
                if (currentAbility.Weapon.name == "Bow")
                {
                    GetComponent<Animator>().SetTrigger("ArrowImpact");
                }
                else if (currentAbility.Weapon.name == "Magic")
                {
                    GetComponent<Animator>().SetTrigger("FireballImpact");
                }

                var enemyHealthSystem = Target.GetComponentInParent<HealthSystem>();

                enemyHealthSystem.TakeDamage(damage);
                rigidBody.velocity = Vector2.zero;
                Target = null;
            }
        }
    }
}