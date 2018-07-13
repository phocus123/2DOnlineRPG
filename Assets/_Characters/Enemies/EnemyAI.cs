using UnityEngine;
using RPG.Core;
using System.Collections;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : CharacterController
    {
        [SerializeField] float aggroRange;
        [SerializeField] int experienceWorth;
        [SerializeField] EnemyRange enemyRange;

        enum State { Idle, Attack, Follow, Retreat}
        State state = State.Idle;

        GameObject target;
        Character character;
        HealthSystem healthSystem;
        WeaponSystem weaponSystem;
        GameManager gameManager;
        AbilitySystem abilitySystem;

        Weapon currentWeapon;
        Vector2 direction;
        Vector3 startPosition;
        float distanceToPlayer;
        float currentWeaponRange;
        float distanceToStartPos;
        float lastHitTime;
        int exitIndex;
        bool inWeaponRange;
        bool inAggroRange;
        bool outsideAggroRange;

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

        public void Update()
        {
            SetExitIndex();
            weaponSystem = GetComponent<WeaponSystem>();
            currentWeapon = weaponSystem.GetWeaponAtIndex(0);
            currentWeaponRange = weaponSystem.GetWeaponAtIndex(0).AttackRange;
            direction = Vector2.zero;

            if (target != null)
            {
                distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
                inWeaponRange = distanceToPlayer <= currentWeaponRange;
                inAggroRange = distanceToPlayer > currentWeaponRange && distanceToPlayer <= aggroRange;
                outsideAggroRange = distanceToPlayer > aggroRange;
            }

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

        // TODO Is this the appropriate place to put this?
        public void AwardExperience(GameObject awardTarget)
        {
            gameManager.PlayerExperience += experienceWorth;
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

        void SetTarget(GameObject collisionObject)
        {
            target = collisionObject;
        }

        void SetAggroRangeCollider()
        {
            var rangeCollider = enemyRange.GetComponent<CircleCollider2D>();
            rangeCollider.radius = aggroRange;
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
                float timeToWait = abilityBehaviour.Ability.AttackSpeed;
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain && !character.IsAttacking)
                {
                    abilitySystem.AttemptAbility(abilityBehaviour, target);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
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