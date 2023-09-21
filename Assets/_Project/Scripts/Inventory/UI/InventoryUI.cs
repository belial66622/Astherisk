using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventoryItemIconUI iconPrefab;

        public List<InventoryItem> inventoryItems = new List<InventoryItem>();

        public Inventory inventory;

        private void Start()
        {
            inventory = Inventory.Instance;
            inventory.OnInventoryChanged += Instance_OnInventoryChanged;
            PopulateInventoryIconUI();
        }

        private void Instance_OnInventoryChanged()
        {
            inventoryItems.Clear();
            foreach (InventoryItem item in Inventory.Instance.GetInventoryItems()) 
            { 
                inventoryItems.Add(item);
            }

            PopulateInventoryIconUI();
        }

        void PopulateInventoryIconUI()
        {
            foreach (Transform icon in transform)
            {
                Destroy(icon.gameObject);
            }
            Debug.Log("icon destroyed");


            Debug.Log("icon <= 0");
            if (inventoryItems.Count <= 0) return;
            Debug.Log("icon > 1");

            foreach(InventoryItem item in inventoryItems)
            {
                var iconInstance = Instantiate(iconPrefab, transform);
                iconInstance.Setup(item.GetItemIcon(), item.name);
            }
            Debug.Log("Populate icon");
        }
    }
}
