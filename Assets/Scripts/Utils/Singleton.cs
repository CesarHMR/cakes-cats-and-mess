using UnityEngine;

namespace utils
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError(typeof(T).ToString() + " is missing.");
                }

                return _instance;
            }
        }



        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Debug.LogWarning("Second instance of " + typeof(T) + " created. Automatic self-destruct triggered.");
                Destroy(this.gameObject);
            }
        }
    }
}