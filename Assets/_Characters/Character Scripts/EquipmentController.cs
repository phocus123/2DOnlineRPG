using UnityEngine;
using RPG.Characters;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RPG.Characters
{
    public class EquipmentController : MonoBehaviour
    {
        [SerializeField] Inventory inventory;
        [SerializeField] EquipmentPanel equipmentPanel;
        [SerializeField] Image draggableItem;
        [Space]
        [SerializeField] List<EquippableItem> startingEquippedItems;

        CharacterManager characterManager;
        ItemSlot draggedSlot;

        public Action<EquippableItem> OnItemEquipped;

        void Awake()
        {
            characterManager = GetComponent<CharacterManager>();

            inventory.OnRightClickEvent += Equip;
            equipmentPanel.OnRightClickEvent += UnEquip;
            inventory.OnBeginDragEvent += BeginDrag;
            equipmentPanel.OnBeginDragEvent += BeginDrag;
            inventory.OnEndDragEvent += EndDrag;
            equipmentPanel.OnEndDragEvent += EndDrag;
            inventory.OnDragEvent += Drag;
            equipmentPanel.OnDragEvent += Drag;
            inventory.OnDropEvent += Drop;
            equipmentPanel.OnDropEvent += Drop;
        }

        void Start()
        {
            SetStartingItems();
        }

        public void Equip(EquippableItem item)
        {
            if (inventory.RemoveItem(item))
            {
                EquippableItem previousItem;
                if (equipmentPanel.AddItem(item, out previousItem))
                {
                    if (previousItem != null)
                    {
                        inventory.AddItem(previousItem);
                        previousItem.UnEquip(characterManager);
                        characterManager.StatPanel.UpdateStatValues();
                    }
                    OnItemEquipped(item);
                    item.Equip(characterManager);
                    characterManager.StatPanel.UpdateStatValues();
                }
                else
                {
                    inventory.AddItem(item);
                }
            }
        }

        public void UnEquip(EquippableItem item)
        {
            if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
            {
                item.UnEquip(characterManager);
                characterManager.StatPanel.UpdateStatValues();
                inventory.AddItem(item);
            }
        }

        // TODO Rework this later to only be used when a character first logs in. A different system will be needed to ensure previously equipped items from last login remain equipped.
        public void EquipStartingItems(EquippableItem item)
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                OnItemEquipped(item);
                item.Equip(characterManager);
                characterManager.StatPanel.UpdateStatValues();
            }
        }

        void SetStartingItems()
        {
            for (int i = 0; i < startingEquippedItems.Count; i++)
            {
                EquipStartingItems(startingEquippedItems[i]);
            }
        }

        // TODO Remove this when serialization of player items is developed.
        private void OnApplicationQuit()
        {
            for (int i = 0; i < startingEquippedItems.Count; i++)
            {
                startingEquippedItems[i].UnEquip(characterManager);
            }
        }

        void Equip(ItemSlot itemSlot)
        {
            EquippableItem equippableItem = itemSlot.Item as EquippableItem;

            if (equippableItem != null)
            {
                Equip(equippableItem);
            }
        }

        void UnEquip(ItemSlot itemSlot)
        {
            EquippableItem equippableItem = itemSlot.Item as EquippableItem;

            if (equippableItem != null)
            {
                UnEquip(equippableItem);
            }
        }

        void BeginDrag(ItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                draggedSlot = itemSlot;
                draggableItem.sprite = itemSlot.Item.icon;
                draggableItem.transform.position = Input.mousePosition;
                draggableItem.enabled = true;
            }
        }

        void EndDrag(ItemSlot itemSlot)
        {
            draggedSlot = null;
            draggableItem.enabled = false;
        }

        void Drag(ItemSlot itemSlot)
        {
            if (draggableItem.enabled)
            {
                draggableItem.transform.position = Input.mousePosition;
            }
        }

        void Drop(ItemSlot dropItemSlot)
        {
            if (draggedSlot == null) return;

            if (dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
            {
                EquippableItem dragItem = draggedSlot.Item as EquippableItem;
                EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

                if (draggedSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        dragItem.UnEquip(characterManager);
                    }
                    if (dropItem != null)
                    {
                        dropItem.Equip(characterManager);
                    }
                }

                if (dropItemSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        OnItemEquipped(dragItem);
                        dragItem.Equip(characterManager);
                    }
                    if (dropItem != null)
                    {
                        dropItem.UnEquip(characterManager);
                    }
                }
                characterManager.StatPanel.UpdateStatValues();

                Item draggedItem = draggedSlot.Item;
                draggedSlot.Item = dropItemSlot.Item;
                dropItemSlot.Item = draggedItem;
            }
        }
    }
}