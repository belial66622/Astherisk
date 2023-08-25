using System.Collections.Generic;
using UnityEngine;

namespace ThePatient
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] List<IPickupable> items = new List<IPickupable>();
        
        public void AddItem(IPickupable item)
        {
            if (HasItem(item)) return;

            items.Add(item);
        }

        public void RemoveItem(IPickupable item)
        {
            if (HasItem(item)) items.Remove(item);
        }

        public bool HasItem(IPickupable item)
        {
            return items.Contains(item);
        }
    }
}
