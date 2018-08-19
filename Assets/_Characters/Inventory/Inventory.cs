using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] List<Item> startingItems;
        [SerializeField] Transform itemsParent;
        [SerializeField] ItemSlot[] itemSlots;

        public event Action<ItemSlot> OnPointerEnterEvent;
        public event Action<ItemSlot> OnPointerExitEvent;
        public event Action<ItemSlot> OnRightClickEvent;
        public event Action<ItemSlot> OnBeginDragEvent;
        public event Action<ItemSlot> OnEndDragEvent;
        public event Action<ItemSlot> OnDragEvent;
        public event Action<ItemSlot> OnDropEvent;

        void OnValidate()
        {
            if (itemsParent != null)
            {
                itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
            }

            SetStartingItems();
        }

        void Start()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].OnPointerEnterEvent += OnPointerEnterEvent;
                itemSlots[i].OnPointerExitEvent += OnPointerExitEvent;
                itemSlots[i].OnRightClickEvent += OnRightClickEvent;
                itemSlots[i].OnBeginDragEvent += OnBeginDragEvent;
                itemSlots[i].OnEndDragEvent += OnEndDragEvent;
                itemSlots[i].OnDragEvent += OnDragEvent;
                itemSlots[i].OnDropEvent += OnDropEvent;
            }
            SetStartingItems();
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = item;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == item)
                {
                    itemSlots[i].Item = null;
                    return true;
                }
            }
            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    return false;
                }
            }
            return true;
        }

        void SetStartingItems()
        {
            int i = 0;
            for (; i  < startingItems.Count && i < itemSlots.Length; i++)
            {
                itemSlots[i].Item = startingItems[i];
            }

            for (; i < itemSlots.Length; i++)
            {
                itemSlots[i].Item = null;
            }
        }
    }
}