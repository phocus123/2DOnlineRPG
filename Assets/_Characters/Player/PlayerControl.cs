using UnityEngine;
using RPG.CameraUI;
using RPG.Core;
using System;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        GameObject target;
        GameObject originalTarget; // Used to track original target before when a spell is cast and then the player clicks on a new target.

        Vector2 direction;
        int exitIndex;
        Vector3 min, max;
        UIManager uiManager;

        void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
            FindObjectOfType<CameraRaycaster>().InvokeOnMouseOverInteractable += OnMouseOverInteractable;
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
            var keybindManager = FindObjectOfType<KeybindManager>();
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

        void RegisterForMouseEvents()
        {
            var cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
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

        void ActivateAttack(AbilityBehaviour abilityBehaviour)
        {
            var abilitySystem = GetComponent<AbilitySystem>();
            var character = GetComponent<Character>();
            var weaponSystem = GetComponent<WeaponSystem>();

            weaponSystem.Block(exitIndex);

            if (target != null && weaponSystem.IsTargetInRange(abilityBehaviour.Ability.Weapon, target) && !character.IsAttacking && !character.IsMoving && weaponSystem.InLineOfSight(target))
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

        void OnMouseOverInteractable(NPCControl npc)
        {
            if (Input.GetMouseButtonDown(1) && npc.IsPlayerInRange)
            {
                uiManager.OpenDialogue(npc);
            }
        }
    }
}