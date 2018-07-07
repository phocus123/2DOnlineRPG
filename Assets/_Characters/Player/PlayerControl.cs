using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        [SerializeField] GameObject target;

        GameManager gameManager;
        Vector2 direction;
        private Vector3 min, max;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        void Update()
        {
            GetInput();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public override Vector2 GetDirection()
        {
            return direction;
        }

        public void GetInput()
        {
            direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                //exitIndex = 0;
                direction += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                //exitIndex = 3;
                direction += Vector2.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                //exitIndex = 2;
                direction += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                //exitIndex = 1;
                direction += Vector2.right;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var healthSystem = target.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(10f);
            }
        }

        public void SetLimits(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }
    }
}