using UnityEngine;

public class SingletonMonoPersistence<T> : SingletonBehaviour<SingletonMonoPersistence<T>> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}