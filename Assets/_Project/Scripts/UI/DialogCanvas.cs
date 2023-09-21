using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogCanvas : SingletonBehaviour<DialogCanvas>
{
    [field: SerializeField]public TextMeshProUGUI speaker { get; private set; }
    [field: SerializeField] public TextMeshProUGUI dialogue { get; private set; }

    private void Start()
    {
        gameObject.SetActive(false);    
    }
}
