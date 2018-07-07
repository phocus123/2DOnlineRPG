using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Characters
{
    public class GameManager : MonoBehaviour
    {
        //public delegate void OnMouseOverEnemy(EnemyControl enemy);
        //public event OnMouseOverEnemy InvokeOnMouseOverEnemy;
        //public delegate void OnMouseOverNormal();
        //public event OnMouseOverNormal InvokeOnMouseOverNormal;
        //public delegate void OnMouseOverInteractable(NPC npc);
        //public event OnMouseOverInteractable InvokeOnMouseOverInteractable;

        //[SerializeField] private Player player = null;

        [SerializeField] private Texture2D enemyCursor = null;
        [SerializeField] private Texture2D interactableCursor = null;
        [SerializeField] private Texture2D mainCursor = null;

        Vector2 cursorHotspot = new Vector2(0, 0);
        const int INTERACTABLE_LAYER = 8;

        //private CharacterOld currentTarget;

        private void Start()
        {
            //InvokeOnMouseOverEnemy += new OnMouseOverEnemy(SelectTarget);
            //InvokeOnMouseOverNormal += new OnMouseOverNormal(DeSelectTarget);
            //InvokeOnMouseOverInteractable += new OnMouseOverInteractable(TriggerDialogue);
        }

        private void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PerformRaycasts();
            }

            //RegenHealth(0.25f);
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
            Cursor.SetCursor(mainCursor, cursorHotspot, CursorMode.Auto);
            //InvokeOnMouseOverNormal();
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
                    //InvokeOnMouseOverEnemy(enemyHit);
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    return true;
                }
            }
            return false;
        }

        private void SelectTarget(EnemyControl enemy)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (currentTarget != null)
            //    {
            //        currentTarget.DeSelect();
            //    }
            //    currentTarget = enemy;

            //    player.Target = currentTarget.Select();

            //    UIManager.Instance.ShowTargetFrame(currentTarget);
            //}
        }

        private void DeSelectTarget()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if (currentTarget != null)
                //{
                //    currentTarget.DeSelect();
                //    UIManager.Instance.HideTargetFrame();
                //}
            }
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

        //private void RegenHealth(float value)
        //{
        //    player.Health.CurrentValue += value * Time.deltaTime;
        //}

        //private void RegenEnergy(float value)
        //{
        //    player.Energy.CurrentValue += value * Time.deltaTime;
        //}
    }
}