using UnityEngine;

public class Singleton<T> : AssassinBehaviour where T : AssassinBehaviour
{
    [SerializeField] private bool dontDestroyOnLoad;

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T) + " (Singleton)");
                    _instance = singletonObject.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}