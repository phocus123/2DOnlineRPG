using RPG.CameraUI;
using RPG.Core;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        GameObject target;
        GameObject originalTarget; // Used to track original target before when a spell is cast and then the player clicks on a new target.

        Vector2 direction;
        int exitIndex;
        Vector3 min, max;
        UIManager uiManager;

        public static event Action OnEscapeKeyDown = delegate { };
        public event Action<DirectionParams> OnDirectionChanged = delegate { };

        void Awake()
        {
            uiManager = GameManager.Instance.uIManager;
            Camera.main.GetComponent<CameraRaycaster>().InvokeOnMouseOverInteractable += OnMouseOverInteractable;
            RegisterForMouseEvents();
            RegisterForActionButtonClicks();
        }

        void Update()
        {
            GetInput();
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        }

        public void GetInput()
        {
            var keybindManager = GameManager.Instance.keybindManager;
            direction = Vector2.zero;
            OnDirectionChanged(new DirectionParams(direction));

            if (Input.GetKey(keybindManager.Keybinds["UP"]))
            {
                exitIndex = 0;
                direction += Vector2.up;
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
            }
            if (Input.GetKey(keybindManager.Keybinds["LEFT"]))
            {
                exitIndex = 3;
                direction += Vector2.left;
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
            }
            if (Input.GetKey(keybindManager.Keybinds["DOWN"]))
            {
                exitIndex = 2;
                direction += Vector2.down;
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
            }
            if (Input.GetKey(keybindManager.Keybinds["RIGHT"]))
            {
                exitIndex = 1;
                direction += Vector2.right;
                OnDirectionChanged(new DirectionParams(direction, exitIndex));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscapeKeyDown();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                UIHelper.ToggleCanvasGroup(uiManager.CharacterPanel);
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
            var abilityController = GetComponent<AbilityController>();
            var characterManager = GetComponent<Character>();
            var weaponController = GetComponent<WeaponController>();

            weaponController.Block(exitIndex);

            originalTarget = target;
            abilityController.AttemptAbility(abilityBehaviour, originalTarget);
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
                uiManager.dialogueUI.OpenDialogue(npc);
            }
        }
    }
}