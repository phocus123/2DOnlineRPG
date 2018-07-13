using UnityEngine;
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        public GameManager gameManager;
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
            RegisterForActionButtonClicks();
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

        public override DirectionParams GetDirectionParams()
        {
            DirectionParams directionParams = new DirectionParams(direction, exitIndex);
            return directionParams;
        }

        public void GetInput()
        {
            var keybindManager = gameManager.keybindManager;
            direction = Vector2.zero;

            if (Input.GetKey(keybindManager.Keybinds["UP"]))
            {
                exitIndex = 0;
                direction += Vector2.up;
            }
            if (Input.GetKey(keybindManager.Keybinds["LEFT"]))
            {
                exitIndex = 3;
                direction += Vector2.left;
            }
            if (Input.GetKey(keybindManager.Keybinds["DOWN"]))
            {
                exitIndex = 2;
                direction += Vector2.down;
            }
            if (Input.GetKey(keybindManager.Keybinds["RIGHT"]))
            {
                exitIndex = 1;
                direction += Vector2.right;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiManager.ToggleInGameMenu();
                uiManager.CloseCanvases();
            }

            foreach (string action in keybindManager.Actionbinds.Keys)
            {
                if (Input.GetKeyDown(keybindManager.Actionbinds[action]))
                {
                    uiManager.ClickActionButton(action);
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
            uiManager = gameManager.uiManager;
            abilitySystem = GetComponent<AbilitySystem>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void RegisterForMouseEvents()
        {
            cameraRaycaster.InvokeOnMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.InvokeOnMouseOverNonEnemy += OnMouseOverNonEnemy;
        }

        void RegisterForActionButtonClicks()
        {
            foreach (ActionButton actionButton in uiManager.ActionButtons)
            {
                actionButton.InvokeOnActionButtonClicked += ActivateAttack;
            }
        }

        // TODO Move to a more appropriate script.
        void ActivateAttack(AbilityBehaviour abilityBehaviour)
        {
            Block();

            if (target != null && IsTargetInRange(abilityBehaviour.Ability.Weapon, target) && !character.IsAttacking && !character.IsMoving && InLineOfSight())
            {
                originalTarget = target;
                abilitySystem.AttemptAbility(abilityBehaviour, originalTarget);
            }
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