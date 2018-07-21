using RPG.Core;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(AbilitySystem))]
    public class EnemyAI : CharacterController
    {
        [SerializeField] int experienceWorth;
        [SerializeField] EnemyRange enemyRange;
        [SerializeField] float aggroRange;

        GameObject target;
        Character character;
        GameManager gameManager;
        HealthSystem healthSystem;
        WeaponSystem weaponSystem;
        AbilitySystem abilitySystem;
        Vector3 startPosition;
        Vector2 direction;

        int exitIndex;
        Weapon currentWeapon;
        float distanceToPlayer;
        float currentWeaponRange;
        float distanceToStartPos;
        float lastHitTime;
        bool inWeaponRange;
        bool inAggroRange;
        bool outsideAggroRange;

        enum State { Idle, Attack, Follow, Retreat }
        State state = State.Idle;

        public GameObject Target { get { return target; } }

        private void Awake()
        {
            character = GetComponent<Character>();
            healthSystem = GetComponent<HealthSystem>();
            gameManager = FindObjectOfType<GameManager>();
            abilitySystem = GetComponent<AbilitySystem>();

            SetAggroRangeCollider();
            enemyRange.InvokeOnAggroRangeEntered += SetTarget;
            startPosition = transform.position;
        }

        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(direction, exitIndex);
            return directionParams;
        }

        void Update()
        {
            direction = Vector2.zero;
            SetExitIndex();
            OnUpdateGetWeapon();
            OnUpdateGetRanges();
            OnUpdateQueryStates();
        }

        void OnUpdateGetWeapon()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            currentWeapon = weaponSystem.GetWeaponAtIndex(0);
            currentWeaponRange = weaponSystem.GetWeaponAtIndex(0).AttackRange;
        }

        void OnUpdateGetRanges()
        {
            if (target != null)
            {
                // TODO Implement always face target when attacking.
                distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
                inWeaponRange = distanceToPlayer <= currentWeaponRange;
                inAggroRange = distanceToPlayer > currentWeaponRange && distanceToPlayer <= aggroRange;
                outsideAggroRange = distanceToPlayer > aggroRange;
            }
        }

        void OnUpdateQueryStates()
        {
            if (inAggroRange)
            {
                ExecuteFollowState();
            }
            if (inWeaponRange)
            {
                ExecuteAttackState();
            }
            if (outsideAggroRange && state != State.Idle)
            {
                ExecuteRetreatState();
            }
            if (outsideAggroRange && state == State.Retreat)
            {
                if (distanceToStartPos <= 0.1)
                {
                    ExecuteIdleState();
                }
            }
        }

        void ExecuteIdleState()
        {
            StopAllCoroutines();
            state = State.Idle;
            Reset();
        }

        void ExecuteRetreatState()
        {
            StopAllCoroutines();
            state = State.Retreat;
            StartCoroutine(Retreat());
        }

        void ExecuteAttackState()
        {
            StopAllCoroutines();
            state = State.Attack;
            var behaviour = abilitySystem.EquippedAbilityBehaviours[0];
            StartCoroutine(AttackTargetRepeatedly(behaviour));
        }

        void ExecuteFollowState()
        {
            StopAllCoroutines();
            state = State.Follow;
            StartCoroutine(ChasePlayer());
        }

        IEnumerator ChasePlayer()
        {
            while (distanceToPlayer >= currentWeaponRange)
            {
                direction = (target.transform.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator Retreat()
        {
            distanceToStartPos = Vector2.Distance(startPosition, transform.position);

            while (distanceToStartPos > 0)
            {
                direction = (startPosition - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, startPosition, 0);
                yield return new WaitForEndOfFrame();
            }
        }

        void Reset()
        {
            direction = Vector2.zero;
            target = null;
            healthSystem.CurrentHealthPoints = healthSystem.MaxHealthPoints;
        }

        IEnumerator AttackTargetRepeatedly(AbilityBehaviour abilityBehaviour)
        {
            bool attackerStillAlive = healthSystem.HealthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().HealthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                float timeToWait = abilityBehaviour.Ability.AttackSpeed.Value;
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain && !character.IsAttacking)
                {
                    abilitySystem.AttemptAbility(abilityBehaviour, target);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        public void AwardExperience(GameObject awardTarget)
        {
            gameManager.PlayerExperience += experienceWorth;
        }

        void SetExitIndex()
        {
            if (direction == Vector2.up)
            {
                exitIndex = 0;
            }
            else if (direction == Vector2.up)
            {
                exitIndex = 3;
            }
            else if (direction == Vector2.down)
            {
                exitIndex = 2;
            }
            else
            {
                exitIndex = 1;
            }
        }

        void SetTarget(GameObject collisionObject)
        {
            target = collisionObject;
        }

        void SetAggroRangeCollider()
        {
            var rangeCollider = enemyRange.GetComponent<CircleCollider2D>();
            rangeCollider.radius = aggroRange;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
        }
    }
}