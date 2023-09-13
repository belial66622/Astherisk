using System;

namespace ThePatient
{
    public interface IPickupable
    {
        void Pickup(string pickupAudio);
        InventoryItem GetItem();
    }
}
