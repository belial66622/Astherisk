using UnityEngine;

namespace ThePatient
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] string itemID;

        InspectPickup inspectObject;
        public void SetItem(InspectPickup inspectObject)
        {
            this.inspectObject = inspectObject;
        }

        public string GetItemID() => itemID;

        public InspectPickup GetObject() => inspectObject;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // do nothing
        }
    }
}
