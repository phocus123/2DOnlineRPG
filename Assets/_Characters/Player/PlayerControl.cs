using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        [SerializeField] Block[] blockArray;

        GameObject target;
        GameObject originalTarget; // Used to track original target before when a spell is cast and then the player clicks on a new target.

        Vector2 direction;
        int exitIndex;
        Vector3 min, max;

        CameraRaycaster cameraRaycaster;
        UIManager uiManager;
        AbilitySystem abilitySystem;
        WeaponSystem weaponSystem;
        Character character;

        void Awake()
        {
            GetRequiredReferences();
            RegisterForMouseEvents();
        }

        void Update()
        {
            GetInput();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
            if (target != null)
            {

            }
                Debug.DrawRay(transform.position, direction * 10, Color.red);
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(direction, exitIndex);
            return directionParams;
        }

        public void GetInput()
        {
            direction = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                exitIndex = 0;
                direction += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                exitIndex = 3;
                direction += Vector2.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                exitIndex = 2;
                direction += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                exitIndex = 1;
                direction += Vector2.right;
            }
            // TODO Remove when damage has been implemented.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Block();

                if (target != null && IsTargetInRange(weaponSystem.GetWeaponAtIndex(0), target) && !character.IsAttacking && !character.IsMoving && InLineOfSight())
                {
                    originalTarget = target;
                    abilitySystem.AttemptAbility(0, originalTarget);
                }
            }
        }

        public void SetLimits(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        // TODO Implement Weapon system
        bool IsTargetInRange(Weapon weapon, GameObject target)
        {
            float distance = Vector2.Distance(target.transform.position, transform.position);
            return distance <= weapon.AttackRange;
        }

        void GetRequiredReferences()
        {
            character = GetComponent<Character>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            uiManager = FindObjectOfType<UIManager>();
            abilitySystem = GetComponent<AbilitySystem>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void RegisterForMouseEvents()
        {
            cameraRaycaster.InvokeOnMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.InvokeOnMouseOverNonEnemy += OnMouseOverNonEnemy;
        }

        void OnMouseOverEnemy(GameObject enemy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (target != null)
                {
                    target = null;
                    uiManager.HideTargetFrame();
                }
                uiManager.ShowTargetFrame(enemy);
                target = enemy;
            }
        }

        void OnMouseOverNonEnemy()
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = null;
                uiManager.HideTargetFrame();
            }
        }

        // TODO Is there a better place to put these?
        void Block()
        {
            foreach (Block b in blockArray)
            {
                b.Deactivate();
            }

            blockArray[exitIndex].Activate();
        }

        bool InLineOfSight()
        {
            if (target != null)
            {
                Vector3 targetDirection = (target.transform.position - transform.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, target.transform.position), 256);

                if (hit.collider == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}