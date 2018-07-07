using UnityEngine;

namespace RPG.Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController runtimeAnimatorController;

        [Header("Obstacle Collider")]
        [SerializeField] Vector2 colliderSize;
        [SerializeField] Vector2 colliderOffset;

        [Header("Movement")]
        [SerializeField] float moveSpeed;
        public float MoveSpeed { get { return moveSpeed; } }

        Animator animator;
        Rigidbody2D rigidBody;
        BoxCollider2D obstacleCollider;
        CharacterController characterController;

        public bool IsMoving
        {
            get
            {
                return characterController.GetDirection().x != 0 || characterController.GetDirection().y != 0;
            }
        }

        bool isAlive = true;


        private void Awake()
        {
            AddRequiredComponents();
        }

        private void Update()
        {
            HandleLayers();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = runtimeAnimatorController;

            rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0;
            rigidBody.freezeRotation = true;

            obstacleCollider = gameObject.AddComponent<BoxCollider2D>();
            obstacleCollider.size = colliderSize;
            obstacleCollider.offset = colliderOffset;

            GetCharacterController();
        }

        void GetCharacterController()
        {
            if (gameObject.GetComponent<PlayerControl>())
            {
                characterController = GetComponent<PlayerControl>();
            }
            else
            {
                characterController = GetComponent<EnemyControl>();
            }
        }

        void Move()
        {
            rigidBody.velocity = characterController.GetDirection().normalized * moveSpeed;
        }

        void HandleLayers()
        {
            if (isAlive)
            {
                if (IsMoving)
                {
                    ActivateLayer("WalkLayer");

                    animator.SetFloat("x", characterController.GetDirection().x);
                    animator.SetFloat("y", characterController.GetDirection().y);
                }
                //else if (IsAttacking)
                //{
                //    ActivateLayer("AttackLayer");
                //}
                else
                {
                    ActivateLayer("IdleLayer");
                }
            }
            //else
            //{
            //    ActivateLayer("DeathLayer");
            //}
        }

        public void ActivateLayer(string layerName)
        {
            for (int i = 0; i < animator.layerCount; i++)
            {
                animator.SetLayerWeight(i, 0);
            }

            animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
        }
    }
}