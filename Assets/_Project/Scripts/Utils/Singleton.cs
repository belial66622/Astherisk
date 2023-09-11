public class Singleton<T> where T : class, new()
{
    private static T _instance;
    private static readonly object _padlock = new object();
    public static T Instance
    {
        get
        {
            lock (_padlock)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
