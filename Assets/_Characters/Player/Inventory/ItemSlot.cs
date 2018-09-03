using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        [SerializeField] Image image;
        [SerializeField] ItemTooltip itemTooltip;

        public event Action<ItemSlot> OnPointerEnterEvent = delegate { };
        public event Action<ItemSlot> OnPointerExitEvent = delegate { };
        public event Action<ItemSlot> OnRightClickEvent = delegate { };
        public event Action<ItemSlot> OnBeginDragEvent = delegate { };
        public event Action<ItemSlot> OnEndDragEvent = delegate { };
        public event Action<ItemSlot> OnDragEvent = delegate { };
        public event Action<ItemSlot> OnDropEvent = delegate { };

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
                OnRightClickEvent(this);
            }
        }

        protected virtual void OnValidate()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            if (itemTooltip == null)
            {
                itemTooltip = FindObjectOfType<ItemTooltip>();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent(this);

            if (item is EquippableItem)
            {
                itemTooltip.ShowTooltip((EquippableItem)item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent(this);
            itemTooltip.HideTooltip();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDragEvent(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropEvent(this);
        }
    }
}