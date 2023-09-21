using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThePatient
{
    public class InventoryItemIconUI : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI iconName;

        public void Setup(Sprite icon, string itemName)
        {
            this.icon.sprite = icon;
            iconName.text = itemName;
        }
    }
}
