using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.Core;
using RPG.Characters;

namespace RPG.CameraUI
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        GameManager gameManager;
        GameObject draggingIcon;

        public IMoveable Moveable { get; set; }

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            draggingIcon = new GameObject("Icon");
            var parent = GameObject.FindGameObjectWithTag("DragIconCanvas").transform;
            var image = draggingIcon.AddComponent<Image>();

            image.raycastTarget = false;
            image.sprite = GetComponent<Image>().sprite;

            // Will need to change this in the future, a universal icon size might be an idea
            image.rectTransform.sizeDelta = new Vector2(45f, 45f); ;
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
            var uiManager = gameManager.uiManager;
            if (draggingIcon != null)
            {
                for (int i = 0; i < uiManager.ActionButtons.Length; i++)
                {
                    if (eventData.pointerDrag.transform.parent.name == uiManager.ActionButtons[i].name)
                    {
                        ActionButton button = uiManager.ActionButtons[i];
                        int index = Array.FindIndex(uiManager.ActionButtons, x => x.Button.name == button.name);
                        gameManager.savegameManager.AbilityDict.Remove(index + 1);
                        button.RemoveAbility();
                    }
                }
                Destroy(draggingIcon);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print("test");
        }
    }
}