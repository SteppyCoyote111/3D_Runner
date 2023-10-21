using UnityEngine;

public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindExistingInstance() ?? CreateNewInstance();
            }
            
            return _instance;
        }
    }

    private static T CreateNewInstance()
    {
        var singletonObject = new GameObject {name = $"New {typeof(T)} (Singleton)"};
        return singletonObject.AddComponent<T>();
    }

    private static T FindExistingInstance()
    {
        return GameObject.FindObjectOfType<T>(true);
    }

    private static void Enable()
    {
        if (_instance != null)
        {
            _instance.enabled = true;
        }
    }

    private void Awake()
    {
        var component = GetComponent<T>();
        
        if (_instance == null)
        {
            _instance = component;
        }
        else if (_instance != component)
        {
            Destroy(gameObject);
            return;
        }
        
        Enable();
        Initialize();
    }

    protected virtual void Initialize()
    {
    }
}
