using System;
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
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                GodModeEnabled = !GodModeEnabled;
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                DestroyAllProcessors();
            }
            
            GodMode();
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
            GameUIManager.Instance.DeathScreen.DeathMessageText.text = "You have successfully desactivated the heart";
            GameUIManager.Instance.DeathScreen.Show();
        }

        private void GodMode()
        {
            if (GodModeEnabled)
            {
                Hero.Instance.Health.MaxHealth = 99999f;
                Hero.Instance.Health.CurrentHealth = 99999f;
                Hero.Instance.MultiToolObject.MaxBattery = 99999f;
                Hero.Instance.MultiToolObject.CurrentBattery = 99999f;
            }
            else
            {
                Hero.Instance.Health.MaxHealth = 100f;
                Hero.Instance.Health.CurrentHealth = 100f;
                Hero.Instance.MultiToolObject.MaxBattery = 100f;
                Hero.Instance.MultiToolObject.CurrentBattery = 100f;
            }
        }
        
        public void DestroyAllProcessors()
        {
            numberOfProcessorsTerminated = 4;
        }
        
        #endregion

        #region variables
        
        public Transform playerTransform;

        public Transform respawnPoint;
        
        [SerializeField] private int numberOfProcessorsTerminated;
        
        public bool GodModeEnabled;

        #endregion
    }
}