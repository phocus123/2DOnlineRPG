using UnityEngine;

namespace RPG.Characters
{
    public class CharacterMovementController : MonoBehaviour
    {
        PlayerControl playerControl;
        EnemyAI enemyAI;
        Rigidbody2D rigidBody;
        Vector2 direction;
        int exitIndex;

        public bool IsMoving { get { return direction.x != 0 || direction.y != 0; } }
        public Vector2 Direction { get { return direction; } }
        public int ExitIndex { get { return exitIndex; } }

        private void Start()
        {
            AddRigidbodyComponent();
            RegisterCharacterDirectionEvent();
        }

        void FixedUpdate()
        {
            Move();
        }

        void RegisterCharacterDirectionEvent()
        {
            playerControl = GetComponent<PlayerControl>();
            enemyAI = GetComponent<EnemyAI>();

            if (playerControl)
            {
                playerControl.OnDirectionChanged += SetDirectionParams;
            }

            if (enemyAI)
            {
                enemyAI.OnDirectionChanged += SetDirectionParams;
            }
        }

        void SetDirectionParams(DirectionParams directionParams)
        {
            direction = directionParams.direction;
            exitIndex = directionParams.exitIndex;
        }

        void Move()
        {
            if (rigidBody != null)
            {
                rigidBody.velocity = direction.normalized * GetComponent<CharacterManager>().MoveSpeed;
            }
        }

        void AddRigidbodyComponent()
        {
            rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0;
            rigidBody.freezeRotation = true;

            if (GetComponent<EnemyAI>() || GetComponent<NPCControl>())
            {
                rigidBody.isKinematic = true;
            }
        }
    }

    public struct DirectionParams
    {
        public Vector2 direction;
        public int exitIndex;

        public DirectionParams(Vector2 direction, int exitIndex = 0)
        {
            this.direction = direction;
            this.exitIndex = exitIndex;
        }
    }
}