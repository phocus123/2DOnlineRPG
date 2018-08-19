using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        [SerializeField] Image image;

        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnRightClickEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;

        Color normalColor = Color.white;
        Color disabledColor = Color.clear;

        Item item;

        public Item Item
        {
            get { return item; }
            set
            {
                item = value;

                if (item == null)
                {
                    image.color = disabledColor;
                }
                else
                {
                    image.sprite = item.icon;
                    image.color = normalColor;
                }
            }
        }

        public virtual bool CanReceiveItem(Item item)
        {
            return true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            {
                if (OnRightClickEvent != null)
                {
                    OnRightClickEvent(this);
                }
            }
        }

        protected virtual void OnValidate()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (OnPointerEnterEvent != null)
            {
                OnPointerEnterEvent(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (OnPointerExitEvent != null)
            {
                OnPointerExitEvent(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (OnBeginDragEvent!= null)
            {
                OnBeginDragEvent(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (OnEndDragEvent != null)
            {
                OnEndDragEvent(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnDragEvent != null)
            {
                OnDragEvent(this);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (OnDropEvent != null)
            {
                OnDropEvent(this);
            }
        }
    }
}