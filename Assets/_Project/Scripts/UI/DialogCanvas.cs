using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DialogCanvas : SingletonBehaviour<DialogCanvas>
{
    [field: SerializeField]public TextMeshProUGUI speaker { get; private set; }
    [field: SerializeField] public TextMeshProUGUI dialogue { get; private set; }
    [SerializeField] Transform basePanel;

    private void Start()
    {
        basePanel.gameObject.SetActive(true);
        gameObject.SetActive(false);    
    }
}
