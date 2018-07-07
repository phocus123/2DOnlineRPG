using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;

namespace RPG.Core
{
    public class GameManager : MonoBehaviour
    {
        public delegate void OnEnemyClicked(GameObject enemy);
        public event OnEnemyClicked InvokeOnEnemyClicked;

        public delegate void OnNonEnemyClicked();
        public event OnNonEnemyClicked InvokeOnNonEnemyClicked;

        //public delegate void OnMouseOverInteractable(GameObject npc);
        //public event OnMouseOverInteractable InvokeOnMouseOverInteractable;

        [SerializeField] private Texture2D enemyCursor = null;
        [SerializeField] private Texture2D interactableCursor = null;
        [SerializeField] private Texture2D mainCursor = null;

        Vector2 cursorHotspot = new Vector2(0, 0);
        const int INTERACTABLE_LAYER = 8;
        GameObject currentEnemyTarget;
        HealthSystem playerHealthSystem;

        const float HEALTH_REGEN_AMOUNT = 0.25f;

        private void Start()
        {
            playerHealthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
            //InvokeOnMouseOverInteractable += new OnMouseOverInteractable(TriggerDialogue);
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PerformRaycasts();
            }

            RegenHealth(HEALTH_REGEN_AMOUNT);
            //RegenEnergy(1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void PerformRaycasts()
        {
            if (RaycastForEnemy()) { return; }
            if (RaycastForInteractable()) { return; }
            if (Input.GetMouseButtonDown(0))
            {
                currentEnemyTarget = null;
                InvokeOnNonEnemyClicked();
            }
            Cursor.SetCursor(mainCursor, cursorHotspot, CursorMode.Auto);
        }

        private bool RaycastForInteractable()
        {
            LayerMask interactableLayer = 1 << INTERACTABLE_LAYER;


            bool interactableHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, interactableLayer );
            if (interactableHit)
            {
                //InvokeOnMouseOverInteractable(interactableHit);
                Cursor.SetCursor(interactableCursor, cursorHotspot, CursorMode.Auto);
                return true;
            }
            return false;
        }

        bool RaycastForEnemy()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                var enemyHit = hit.collider.gameObject.GetComponentInParent<EnemyControl>();
                if (enemyHit)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (currentEnemyTarget != null)
                        {
                            InvokeOnNonEnemyClicked();
                        }
                        currentEnemyTarget = enemyHit.gameObject;
                        InvokeOnEnemyClicked(enemyHit.gameObject);
                    }
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    return true;
                }
            }
            return false;
        }

        private void TriggerDialogue(NPCControl npc)
        {
            //if (Input.GetMouseButtonDown(1))
            //{
            //    if (npc.TalkRange)
            //    {
            //        DialogueManager.Instance.CurrentNpc = npc;
            //        DialogueManager.Instance.OpenDialogue(player);
            //    }
            //}
        }

        void RegenHealth(float value)
        {
            playerHealthSystem.CurrentHealthPoints += value * Time.deltaTime;
        }

        //private void RegenEnergy(float value)
        //{
        //    player.Energy.CurrentValue += value * Time.deltaTime;
        //}
    }
}