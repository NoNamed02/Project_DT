using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        var self = this as T;
        if (Instance != null && Instance != self)
        {
            Destroy(gameObject);
            return;
        }
        Instance = self;
    }
}
