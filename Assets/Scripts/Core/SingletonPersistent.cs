using UnityEngine;

namespace Singleton
{
    public abstract class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        // 1. Add this safety flag
        private static bool applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                // 2. If the app is quitting, stop trying to make a new instance!
                if (applicationIsQuitting)
                {
                    // Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Persistent Singleton)";
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        // 3. Mark the flag as true when the app quits
        protected virtual void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        // 4. Mark the flag as true if the persistent object is actively destroyed
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                applicationIsQuitting = true;
            }
        }
    }
}