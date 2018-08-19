using UnityEngine;
using RPG.Characters;
using System;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class EquipmentSystem : MonoBehaviour
    {
        [SerializeField] Inventory inventory;
        [SerializeField] EquipmentPanel equipmentPanel;
        [SerializeField] Image draggableItem;

        CharacterStats characterStats;
        ItemSlot draggedSlot;

        void Awake()
        {
            characterStats = GetComponent<CharacterStats>();

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
                        previousItem.UnEquip(characterStats);
                        characterStats.StatPanel.UpdateStatValues();
                    }
                    item.Equip(characterStats);
                    characterStats.StatPanel.UpdateStatValues();
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
                item.UnEquip(characterStats);
                characterStats.StatPanel.UpdateStatValues();
                inventory.AddItem(item);
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
            if (dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
            {
                EquippableItem dragItem = draggedSlot.Item as EquippableItem;
                EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

                if (draggedSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        dragItem.UnEquip(characterStats);
                    }
                    if (dropItem != null)
                    {
                        dropItem.Equip(characterStats);
                    }
                }

                if (dropItemSlot is EquipmentSlot)
                {
                    if (dragItem != null)
                    {
                        dragItem.Equip(characterStats);
                    }
                    if (dropItem != null)
                    {
                        dropItem.UnEquip(characterStats);
                    }
                }
                characterStats.StatPanel.UpdateStatValues();

                Item draggedItem = draggedSlot.Item;
                draggedSlot.Item = dropItemSlot.Item;
                dropItemSlot.Item = draggedItem;
            }
        }
    }
}