using UnityEngine;

namespace ThePatient
{
    public class InspectPickupObject : InspectPickup
    {
        [SerializeField] private string pickupAudio;

        public override void Pickup(string audio)
        {
            audio = pickupAudio;
            base.Pickup(audio);
        }

    }
}
