using UnityEngine;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class PlayerControl : CharacterController
    {
        GameObject target;
        GameObject originalTarget; // Used to track original target before when a spell is cast and then the player clicks on a new target.

        Vector2 direction;
        int exitIndex;
        Vector3 min, max;

        CameraRaycaster cameraRaycaster;
        UIManager uiManager;
        AbilitySystem abilitySystem;

        void Awake()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            uiManager = FindObjectOfType<UIManager>();
            abilitySystem = GetComponent<AbilitySystem>();

            RegisterForMouseEvents();
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
                originalTarget = target;
                abilitySystem.AttempAbility(1, originalTarget);
            }
        }

        public void SetLimits(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
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
    }
}