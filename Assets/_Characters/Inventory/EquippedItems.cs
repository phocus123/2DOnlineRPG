using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class EquippedItems : MonoBehaviour
    {
        [SerializeField] GearSlot[] gearSlots;
        [SerializeField] List<EquippableItem> startingEquippedItems;

        EquipmentSystem equipmentSystem;

        void OnValidate()
        {
            gearSlots = GetComponentsInChildren<GearSlot>();
        }

        void Awake()
        {
            equipmentSystem = GetComponentInParent<EquipmentSystem>();
        }

        void Start()
        {
            SetStartingItems();
        }

        void SetStartingItems()
        {
            for (int i = 0; i < startingEquippedItems.Count; i++)
            {
                equipmentSystem.EquipStartingItems(startingEquippedItems[i]);
            }
        }
    }
}