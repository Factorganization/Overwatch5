using Cysharp.Threading.Tasks;
using Systems;
using UnityEngine;

namespace GameContent.Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        #region methodes

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private async void Start()
        {
            await WaitForObjectOfTypeAsync<Hero>();
        }
        
        async UniTask WaitForObjectOfTypeAsync<T>() where T : MonoBehaviour
        {
            T found = null;

            while (found == null)
            {
                found = FindObjectOfType<T>();
                await UniTask.Yield(); // yield to next frame
            }
            
            playerTransform = found.transform;
            Debug.Log($"Async found {typeof(T).Name} at position: {transform.position}");
        }
        
        
        #endregion

        #region variables
        
        public Transform playerTransform;

        #endregion
    }
}