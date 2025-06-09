using UnityEngine;

namespace RunTimeContent.Utility
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        #region properties

        public static bool HasInstance => instance != null;
        
        public static T Current => instance;

        public static T Instance
        {
            get
            {
                if (instance is not null)
                    return instance;
                
                instance = FindFirstObjectByType<T>();
                
                if (instance is not null)
                    return instance;
                
                var obj = new GameObject
                {
                    name = typeof(T).Name + "AutoCreated"
                };
                
                instance = obj.AddComponent<T>();

                return instance;
            }
        }
        
        #endregion
        
        #region methods

        protected virtual void Awake() => InitializeSingleton();

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (unparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                enabled = true;
            }
            
            else
            {
                if (this != instance)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        #endregion

        #region  fields
        
        [Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
        public bool unparentOnAwake = true;
        
        private static T instance;

        #endregion
    }
}