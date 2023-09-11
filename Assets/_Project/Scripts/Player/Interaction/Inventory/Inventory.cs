using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    [RequireComponent(typeof(SaveableEntity))]
    public class Inventory : SingletonBehaviour<Inventory>, ISaveable
    {
        List<IPickupable> items = new List<IPickupable>();

        public List<InspectInteractable> itemList = new List<InspectInteractable>();
        public void AddItem(IPickupable item)
        {
            if (HasItem(item)) return;

            //items.Add(item);
            itemList.Add((InspectInteractable)item);
            Debug.Log($"Added {item}");
        }

        public void RemoveItem(IPickupable item)
        {
            if (HasItem(item))
            {
                //items.Remove(item);
                itemList.Remove((InspectInteractable)item);
            }
            Debug.Log($"Removed {item}");
        }

        public bool HasItem(IPickupable item)
        {
            Debug.Log($"{item} Exist.");
            return itemList.Contains((InspectInteractable)item);
        }

        class InventoryRecord
        {
            public string interactableID;
        }

        public object CaptureState()
        {
            var inventoryRecords = new InventoryRecord[itemList.Count];

            for (int i = 0; i < itemList.Count; i++)
            {
                inventoryRecords[i].interactableID = itemList[i].GetInteractableID();
            }

            return inventoryRecords;
        }

        public void RestoreState(object state)
        {
            var stateObject = state as List<InspectInteractable>;
            itemList = stateObject;
        }
    }
}
