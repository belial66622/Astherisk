using UnityEngine;
using UnityEngine.Rendering;

namespace ThePatient
{
    public class InteractableManager : MonoBehaviour
    {
        public static InteractableManager Instance { get; private set; }
        [field: SerializeField] public Transform inspectTransform { get; private set; }
        [SerializeField] InputReader _input;

        [Header("Reference")]
        [SerializeField] GameObject globalVolume;
        [SerializeField] GameObject inspectVolume;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void StartInspecting()
        {
            _input.DisablePlayerControll();
            _input.EnableInspectControl();
            globalVolume.SetActive(false);
            inspectVolume.SetActive(true);
        }

        public void StopInspecting()
        {
            _input.EnablePlayerControll();
            globalVolume.SetActive(true);
            inspectVolume.SetActive(false);
        }
    }
}
