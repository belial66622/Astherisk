using System;
using System.Collections.Generic;
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
        [SerializeField] List<InventoryItem> inventoryItemsDictionary = new List<InventoryItem>();


        public event Action OnInventoryChanged;

        protected override void Awake()
        {
            base.Awake();

            PopulateItemDatabase();
        }

        public void AddItem(InventoryItem item)
        {
            if (HasItem(item)) return;

            inventoryItems.Add(item);
            //items.Add(item);

            OnInventoryChanged?.Invoke();
        }

        public void RemoveItem(InventoryItem item)
        {
            if (HasItem(item))
            {
                inventoryItems.Remove(item);
                //items.Remove(item);
            }
            OnInventoryChanged?.Invoke();
        }

        public void ResetItem()
        {
            inventoryItems.Clear();

            OnInventoryChanged?.Invoke();
        }

        public bool HasItem(InventoryItem item)
        {
            foreach(InventoryItem item2 in inventoryItems)
            {
                if (item.GetItemID() == item2.GetItemID())
                {
                    Debug.Log("item sama");
                    return true;
                }
            }

            Debug.Log("item beda");
            return false;
        }

        public List<InventoryItem> GetInventoryItems()
        {
            return inventoryItems;
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
                inventoryItemsDictionary.Add(item);
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
