using UnityEngine;
using UnityEngine.UI;

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

        [Header("General Character Data")]
        [SerializeField] Text nameText;
        [SerializeField] string characterName;
        [SerializeField] int experienceWorth;
        [SerializeField] Transform[] exitPoints;

        Animator animator;
        Rigidbody2D rigidBody;
        BoxCollider2D obstacleCollider;
        CharacterController characterController;

        bool isAlive = true;
        bool isAttacking;

        public bool IsMoving{ get { return characterController.GetDirectionParams().direction.x != 0 || characterController.GetDirectionParams().direction.y != 0; } }

        public Transform[] ExitPoints { get { return exitPoints; } }
        public int ExitIndex { get { return characterController.GetDirectionParams().exitIndex; } }

        void Awake()
        {
            AddRequiredComponents();
            nameText.text = characterName; // TODO Is this the appropriate place to put this?
        }

        void Update()
        {
            HandleLayers();
        }

        void FixedUpdate()
        {
            Move();
        }

        // TODO re-implement ability parameter.
        public void StartAttackAnimation()
        {
            isAttacking = true;
            //var currentAnimationName = ability.Weapon.AnimationName; // TODO get rid of string reference.
            var currentAnimationName = "Cast";
            animator.SetBool(currentAnimationName, isAttacking);
        }

        public void StopAttackAnimation()
        {
            isAttacking = false;
            var currentAnimationName = "Cast";
            animator.SetBool(currentAnimationName, isAttacking);
        }


        public void KillCharacter()
        {
            isAlive = false;
        }

        void AddRequiredComponents()
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
            else if (gameObject.GetComponent<NPCControl>())
            {
                characterController = GetComponent<NPCControl>();
            }
            else
            {
                characterController = GetComponent<EnemyControl>();
            }
        }

        void Move()
        {
            rigidBody.velocity = characterController.GetDirectionParams().direction.normalized * moveSpeed;
        }

        void HandleLayers()
        {
            if (isAlive)
            {
                if (IsMoving)
                {
                    ActivateLayer("WalkLayer");

                    animator.SetFloat("x", characterController.GetDirectionParams().direction.x);
                    animator.SetFloat("y", characterController.GetDirectionParams().direction.y);
                }
                else if (isAttacking)
                {
                    ActivateLayer("AttackLayer");
                }
                else
                {
                    ActivateLayer("IdleLayer");
                }
            }
            else
            {
                ActivateLayer("DeathLayer");
            }
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