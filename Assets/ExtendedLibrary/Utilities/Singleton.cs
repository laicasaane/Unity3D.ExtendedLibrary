using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    private static bool _automatic = false;
    private static bool _destroyed = false;
    private static bool _missing = false;
    private static bool _persistent = false;

    protected Singleton(bool persistent, bool automatic)
    {
        _persistent = persistent;
        _automatic = automatic;
    }

    public static T Instance
    {
        get
        {
            if (!Application.isPlaying)
            {
                var instances = FindObjectsOfType<T>();

                if (instances.Length == 1)
                {
                    _instance = instances[0];
                }
                else if (instances.Length == 0)
                {
                    _instance = new GameObject("[Singleton] " + typeof(T).Name).AddComponent<T>();
                }
                else if (instances.Length > 1)
                {
                    throw new UnityException("More than one '" + typeof(T).Name + "' singleton in the scene.");
                }
            }

            if (_destroyed)
            {
                return null;
            }

            if (_missing)
            {
                throw new UnityException("Missing '" + typeof(T).Name + "' singleton in the scene.");
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<T>();

                    if (instances.Length == 1)
                    {
                        _instance = instances[0];
                    }
                    else if (instances.Length == 0)
                    {
                        var singleton = new GameObject("[Singleton] " + typeof(T).Name);
                        _instance = singleton.AddComponent<T>();

                        if (!_automatic)
                        {
                            Destroy(singleton);

                            _missing = true;

                            throw new UnityException("Missing '" + typeof(T).Name + "' singleton in the scene.");
                        }

                        if (_persistent)
                        {
                            DontDestroyOnLoad(singleton);
                        }
                    }
                    else if (instances.Length > 1)
                    {
                        throw new UnityException("More than one '" + typeof(T).Name + "' singleton in the scene.");
                    }
                }

                return _instance;
            }
        }
    }

    public static bool Instantiated
    {
        get { return !_missing && !_destroyed && _instance != null; }
    }

    protected virtual void OnDestroy()
    {
        if (_persistent)
        {
            _destroyed = true;
        }
    }
}