using UnityEngine;

public class SingletonMonoPersistence<T> : SingletonMonoBehaviour<SingletonMonoPersistence<T>> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}