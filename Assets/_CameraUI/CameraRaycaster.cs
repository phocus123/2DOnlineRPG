using UnityEngine;
using UnityEngine.EventSystems;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        public delegate void OnMouseOverEnemy(GameObject enemy);
        public event OnMouseOverEnemy InvokeOnMouseOverEnemy;

        public delegate void OnMouseOverNonEnemy();
        public event OnMouseOverNonEnemy InvokeOnMouseOverNonEnemy;

        //public delegate void OnMouseOverInteractable(GameObject npc);
        //public event OnMouseOverInteractable InvokeOnMouseOverInteractable;

        [SerializeField] private Texture2D enemyCursor = null;
        [SerializeField] private Texture2D interactableCursor = null;
        [SerializeField] private Texture2D mainCursor = null;

        Vector2 cursorHotspot = new Vector2(0, 0);
        const int INTERACTABLE_LAYER = 8;

        void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            if (RaycastForEnemy()) { return; }
            if (RaycastForInteractable()) { return; }
            InvokeOnMouseOverNonEnemy();
            Cursor.SetCursor(mainCursor, cursorHotspot, CursorMode.Auto);
        }

        bool RaycastForInteractable()
        {
            LayerMask interactableLayer = 1 << INTERACTABLE_LAYER;

            bool interactableHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, interactableLayer);
            if (interactableHit)
            {
                // TODO re-implement mouse over interactable.

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
                    InvokeOnMouseOverEnemy(enemyHit.gameObject);
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    return true;
                }
            }
            return false;
        }
    }
}