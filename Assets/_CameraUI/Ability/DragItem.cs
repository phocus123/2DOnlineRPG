using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Core;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        GameManager gameManager;
        GameObject draggingIcon;

        public IMoveable Moveable { get; set; }

        void Start()
        {
            gameManager = GameManager.Instance;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            draggingIcon = new GameObject("Icon");
            var parent = GameObject.FindGameObjectWithTag("DragIconCanvas").transform;
            var image = draggingIcon.AddComponent<Image>();

            image.raycastTarget = false;
            image.sprite = GetComponent<Image>().sprite;

            // Will need to change this in the future, a universal icon size might be an idea
            image.rectTransform.sizeDelta = new Vector2(40f, 40f); ;
            draggingIcon.transform.SetParent(parent, false);

            Ability a = Array.Find(gameManager.MasterAbilityList, x => x.Icon == image.sprite);
            Moveable = a;
        }

        public void OnDrag(PointerEventData eventData)
        {
            draggingIcon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (draggingIcon != null)
            {
                for (int i = 0; i < UIManager.Instance.ActionButtons.Length; i++)
                {
                    if (eventData.pointerDrag.name == UIManager.Instance.ActionButtons[i].name)
                    {
                        ActionButton button = UIManager.Instance.ActionButtons[i];
                        int index = Array.FindIndex(UIManager.Instance.ActionButtons, x => x.Button.name == button.name);
                        gameManager.savegameManager.AbilityDict.Remove(index + 1);
                        button.RemoveAbility();
                    }
                }
                Destroy(draggingIcon);
            }
        }
    }
}