using JetBrains.Annotations;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Instance already exists " + Instance.name);
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
    }
}
