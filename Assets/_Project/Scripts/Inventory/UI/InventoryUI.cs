using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] Transform iconParent;
        [SerializeField] InventoryItemIconUI iconPrefab;

        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        void Start()
        {
            Inventory.Instance.OnInventoryChanged += Instance_OnInventoryChanged;
            PopulateInventoryIconUI();
        }

        private void Instance_OnInventoryChanged(List<InventoryItem> obj)
        {
            //inventoryItems.Clear();
            inventoryItems = obj;

            PopulateInventoryIconUI();
        }

        void PopulateInventoryIconUI()
        {
            foreach(Transform icon in iconParent)
            {
                Destroy(icon.gameObject);
            }

            if (inventoryItems.Count <= 0) return;

            foreach(InventoryItem item in inventoryItems)
            {
                var iconInstance = Instantiate(iconPrefab, iconParent);
                iconInstance.Setup(item.GetItemIcon(), item.name);
            }
        }
    }
}
