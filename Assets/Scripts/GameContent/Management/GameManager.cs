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
        
        private async UniTask WaitForObjectOfTypeAsync<T>() where T : MonoBehaviour
        {
            T found = null;

            while (found == null)
            {
                found = FindFirstObjectByType<T>();
                await UniTask.Yield(); // yield to next frame
            }
            
            playerTransform = found.transform;
            Debug.Log($"Async found {typeof(T).Name} at position: {transform.position}");
        }
        
        public void Respawn()
        {
            if (respawnPoint != null)
            {
                Time.timeScale = 1f;
                playerTransform.position = respawnPoint.position;
                playerTransform.rotation = respawnPoint.rotation;
                Debug.Log("Player respawned at checkpoint.");
            }
            else
            {
                Debug.LogWarning("No respawn point set.");
            }
        }
        
        public void TerminateProcessor()
        {
            numberOfProcessorsTerminated++;
        }
        
        public int GetNumberOfProcessorsTerminated()
        {
            return numberOfProcessorsTerminated;
        }
        
        public void WinGame()
        {
            GameUIManager.Instance.DeathScreen.DeathMessageText.text = "You heve successfully desactivated the heart";
            GameUIManager.Instance.DeathScreen.Show();
        }
        
        #endregion

        #region variables
        
        public Transform playerTransform;

        public Transform respawnPoint;
        
        [SerializeField] private int numberOfProcessorsTerminated;

        #endregion
    }
}