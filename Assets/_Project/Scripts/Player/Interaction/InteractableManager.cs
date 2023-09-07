using UnityEngine;
using UnityEngine.Rendering;

namespace ThePatient
{
    public class InteractableManager : MonoBehaviour
    {
        public static InteractableManager Instance { get; private set; }
        [field: SerializeField] public Transform inspectTransform { get; private set; }
        [SerializeField] InputReader _input;
        [field: SerializeField] public float InspectRotateSpeed { get; private set; } = .5f;

        [Header("Reference")]
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
            inspectVolume.SetActive(true);
        }

        public void StopInspecting()
        {
            _input.EnablePlayerControll();
            inspectVolume.SetActive(false);
        }

        public void OnInteractionInspect<T>(T eventArgs)
        {
            EventAggregate<T>.Instance.TriggerEvent(eventArgs);
        }

    }
}
