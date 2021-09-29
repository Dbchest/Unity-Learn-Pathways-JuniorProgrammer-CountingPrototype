using UnityEngine;

[RequireComponent(typeof(DontDestroyOnLoad))]
public abstract class MonoSingleton<T> : MonoBehaviour
    where T : MonoSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogWarning(typeof(T).Name + " is null.");
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Debug.LogWarning(typeof(T).Name + " already exists.");
            Destroy(gameObject);
            Debug.Log(gameObject.name + " has been destroyed.");
        }
        Initialize();
    }

    protected virtual void Initialize()
    {
        print("The single allowable instance of " + typeof(T).Name + " has been initialized.");
    }
}