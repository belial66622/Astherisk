using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                T[] instances = Resources.LoadAll<T>("");
                if (instances == null || instances.Length < 1)
                {
                    throw new System.Exception("No instance of " + typeof(T).Name + " found in Resources folder.");
                }
                else if(instances.Length > 1)
                {
                    throw new System.Exception("Multiple Instances of " + typeof(T).Name + " Found !");
                }
                instance = instances[0];
            }
            return instance;
        }
    }
}