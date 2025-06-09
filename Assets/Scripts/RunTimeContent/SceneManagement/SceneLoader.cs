using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RunTimeContent.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        #region methodes

        private void Awake()
        {
            if (Loader is not null)
                return;

            Loader = this;
            
            _manager.OnSceneLoaded += sceneName => Debug.Log("Loaded : " + sceneName);
            _manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded : " + sceneName);
            _manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        private async void Start()
        {
            await LoadSceneGroup(0);
        }
        
        #region simple loading

        public async Task LoadSceneGroup(int index)
        {
            await SceneGroupManager.NaiveLoadSceneGroup(sceneGroups[index]);
        }

        public async Task UnloadSceneGroup(int index)
        {
            await SceneGroupManager.NaiveUnloadSceneGroup(sceneGroups[index]);
        }
        
        #endregion

        #region canvas loading
        
        private void Update()
        {
            if (!_isLoading) return;
            
            var currentFillAmount = loadingBar.fillAmount;
            var progressDifference = Mathf.Abs(currentFillAmount - _targetProgress);
            
            var dynamicSpeed = progressDifference * fillSpeed;
            
            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, _targetProgress, Time.deltaTime * dynamicSpeed);
        }
        
        public async Task LoadSceneGroupWithCanvas(int index)
        {
            loadingBar.fillAmount = 0f;
            _targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length)
            {
                Debug.LogError("Invalid scene group index : " + index);
                return;
            }
            
            var progress = new LoadingProgress();
            progress.Progressed += target => _targetProgress = Mathf.Max(target, _targetProgress);
            
            EnableLoadingCanvas();
            await _manager.LoadScenes(sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        private void EnableLoadingCanvas(bool enable = true)
        {
            _isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            loadingCamera.gameObject.SetActive(enable);
        }
        
        #endregion
        
        #endregion
        
        #region fields
        
        public static SceneLoader Loader;
        
        private readonly SceneGroupManager _manager = new();
        
        [SerializeField] private Image loadingBar;
        
        [SerializeField] private float fillSpeed = 0.5f;
        
        [SerializeField] private Canvas loadingCanvas;
        
        [SerializeField] private Camera loadingCamera;
        
        [SerializeField] private SceneGroup[] sceneGroups;

        private float _targetProgress;
        
        private bool _isLoading;
        
        #endregion
    }
}