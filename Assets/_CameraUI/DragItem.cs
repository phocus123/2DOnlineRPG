using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.CameraUI
{
    public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //ublic IMoveable Moveable { get; set; }

        private GameObject draggingIcon;

        /// <summary>
        /// Will need to be re written to allow reusability for draggable items other than abilities.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            //draggingIcon = new GameObject("Icon");
            //var parent = GameObject.FindGameObjectWithTag("UI Canvas").transform;
            //var image = draggingIcon.AddComponent<Image>();

            //image.raycastTarget = false;
            //image.sprite = GetComponent<Image>().sprite;

            //// Will need to change this in the future, a universal icon size might be an idea
            //image.rectTransform.sizeDelta = new Vector2(36.7f, 34.4f); ;
            //draggingIcon.transform.SetParent(parent, false);

            //Abilities.Ability a = Array.Find(AbilityManager.Instance.Abilities, x => x.Icon == image.sprite);
            //Moveable = a;
        }

        public void OnDrag(PointerEventData eventData)
        {
           // draggingIcon.transform.position = Input.mousePosition;
        }

        /// <summary>
        /// Remove the useable from the button, also remove from the saved abilities list in order to persist the correct abilities on the action bar.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            //if (draggingIcon != null)
            //{
            //    for (int i = 0; i < UIManager.Instance.ActionButtons.Length; i++)
            //    {
            //        if (eventData.pointerDrag.transform.parent.name == UIManager.Instance.ActionButtons[i].name)
            //        {
            //            ActionButton button = UIManager.Instance.ActionButtons[i];
            //            int index = Array.FindIndex(UIManager.Instance.ActionButtons, x => x.Button.name == button.name);
            //            SaveGameManager.Instance.AbilityDict.Remove(index + 1);
            //            button.RemoveUseable();
            //        }
            //    }
            //    Destroy(draggingIcon);
            //}
        }
    }
}