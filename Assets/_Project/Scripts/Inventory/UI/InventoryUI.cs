using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventoryItemIconUI iconPrefab;

        List<InventoryItem> inventoryItems = new List<InventoryItem>();
        void OnEnable()
        {
            Inventory.Instance.OnInventoryChanged += Instance_OnInventoryChanged;
        }

        private void Start()
        {
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
            if (transform.childCount > 0)
            {
                foreach (Transform icon in transform)
                {
                    Destroy(icon.gameObject);
                }

            }


            if (inventoryItems.Count <= 0) return;

            foreach(InventoryItem item in inventoryItems)
            {
                var iconInstance = Instantiate(iconPrefab, transform);
                iconInstance.Setup(item.GetItemIcon(), item.name);
            }
        }
    }
}
