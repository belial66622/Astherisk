using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ThePatient
{
    public class Inventory : SingletonBehaviour<Inventory>, ISaveable
    {
        [Tooltip("Old List but still here")]
        List<IPickupable> items = new List<IPickupable>();

        [Tooltip("Items that player collect throughout the gameplay")]
        [SerializeField] List<InventoryItem> inventoryItems = new List<InventoryItem>();

        [Tooltip("Every Items that exist in the game")]
        Dictionary<string, InventoryItem> itemDatabase = new Dictionary<string, InventoryItem>();

        protected override void Awake()
        {
            base.Awake();

            PopulateItemDatabase();
        }

        public void AddItem(IPickupable item)
        {
            if (HasItem(item)) return;

            inventoryItems.Add(item.GetItem());
            items.Add(item);
        }

        public void RemoveItem(IPickupable item)
        {
            if (HasItem(item))
            {
                inventoryItems.Remove(item.GetItem());
                items.Remove(item);
            }
        }

        public bool HasItem(IPickupable item)
        {
            return items.Contains(item) || inventoryItems.Contains(item.GetItem());
        }

        public void PopulateItemDatabase()
        {
            var itemList = Resources.LoadAll<InventoryItem>("");
            foreach (var item in itemList)
            {
                if (itemDatabase.ContainsKey(item.GetItemID()))
                {
                    continue;
                }
                Debug.Log(item.GetItemID());
                itemDatabase.Add(item.GetItemID(),item);
            }
        }

        public InventoryItem GetItemFromID(string itemID)
        {
            if (itemDatabase == null)
            {
                PopulateItemDatabase();
            }

            if (!itemDatabase.ContainsKey(itemID) || itemID == null) return null;

            return itemDatabase[itemID];
        }

        [System.Serializable]
        class InventoryItemRecord
        {
            public string ItemID;
        }

        public object CaptureState()
        {
            var state = new List<object>();

            foreach (var item in inventoryItems)
            {
                InventoryItemRecord record = new InventoryItemRecord();
                record.ItemID = item.GetItemID();
                state.Add(record);
            }

            return state;
        }

        public void RestoreState(object state)
        {
            var stateRestore = state as List<object>;

            if (state == null) return;

            foreach (var item in stateRestore)
            {
                InventoryItemRecord record = item as InventoryItemRecord;
                inventoryItems.Add(GetItemFromID(record.ItemID));
            }

            foreach(var item in inventoryItems)
            {
                item.GetObject().gameObject.SetActive(false);
            }
        }
    }
}
