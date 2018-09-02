using RPG.Core;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Characters
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] EnemyRange enemyRange;

        GameObject target;
        CharacterManager characterManager;
        GameManager gameManager;
        HealthController healthController;
        WeaponController weaponController;
        AbilityController abilityController;
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
        float aggroRange;

        enum State { Idle, Attack, Follow, Retreat }
        State state = State.Idle;

        public GameObject Target { get { return target; } }
        public event Action<DirectionParams> OnDirectionChanged = delegate { };

        private void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
            healthController = GetComponent<HealthController>();
            gameManager = FindObjectOfType<GameManager>();
            abilityController = GetComponent<AbilityController>();

            SetAggroRangeCollider();
            enemyRange.InvokeOnAggroRangeEntered += SetTarget;
            startPosition = transform.position;
            aggroRange = characterManager.AggroRange;
        }

        void Update()
        {
            direction = Vector2.zero;
            OnDirectionChanged(new DirectionParams(direction, exitIndex));

            SetExitIndex();
            OnUpdateGetWeapon();
            OnUpdateGetRanges();
            OnUpdateQueryStates();
        }

        void OnUpdateGetWeapon()
        {
            currentWeaponRange = abilityController.Abilities[0].AttackRange;
        }

        void OnUpdateGetRanges()
        {
            if (target != null)
            {
                // TODO Implement always face target when attacking.
                distanceToPlayer = Vector2.Distance(transform.position, target.transform.position);
                inWeaponRange = distanceToPlayer <= currentWeaponRange;
                inAggroRange = distanceToPlayer > currentWeaponRange && distanceToPlayer <= characterManager.AggroRange;
                outsideAggroRange = distanceToPlayer > characterManager.AggroRange;
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
            var behaviour = abilityController.EquippedAbilityBehaviours[0];
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
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
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
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
                transform.position = Vector2.MoveTowards(transform.position, startPosition, 0);
                yield return new WaitForEndOfFrame();
            }
        }

        void Reset()
        {
            direction = Vector2.zero;
            OnDirectionChanged(new DirectionParams(direction, exitIndex));
            target = null;
            healthController.CurrentHealthPoints = characterManager.MaxHealthPoints;
            abilityController.CurrentEnergyPoints = characterManager.MaxEnergyPoints;
        }

        IEnumerator AttackTargetRepeatedly(AbilityBehaviour abilityBehaviour)
        {
            bool attackerStillAlive = healthController.HealthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthController>().HealthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                float timeToWait = abilityBehaviour.Ability.AttackSpeed.Value;
                bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                if (isTimeToHitAgain && !characterManager.IsAttacking)
                {
                    abilityController.AttemptAbility(abilityBehaviour, target);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timeToWait);
            }
        }

        public void AwardExperience(GameObject awardTarget)
        {
            gameManager.PlayerExperience += characterManager.ExperienceWorth;
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
            rangeCollider.radius = characterManager.AggroRange;
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