using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RunTimeContent.SceneManagement
{
    public class SceneGroupManager
    {
        #region events
        
        public event Action<string> OnSceneLoaded = delegate { };
        
        public event Action<string> OnSceneUnloaded = delegate { };
        
        public event Action OnSceneGroupLoaded = delegate { };
        
        #endregion
        
        #region methodes
        
        #region clean UI load
            
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false)
        {
            _activeSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();

            var sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }
            
            var totalScenesToLoad = _activeSceneGroup.scenes.Count;
            
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.scenes[i];
                
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name))
                    continue;
                
                var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);

                await Task.Delay(TimeSpan.FromSeconds(2.5f));
                operationGroup.operations.Add(operation);
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            
            // Wait until all AsyncOperations in the group are done
            while (!operationGroup.IsDone)
            {
                progress?.Report(operationGroup.Progress);
                await Task.Delay(100);
            }
            
            var activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));
            
            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }
            
            OnSceneGroupLoaded.Invoke();
        }
        
        public async Task UnloadScenes()
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            
            var sceneCount = SceneManager.sceneCount;
            
            for (var i = 0; i < sceneCount; i++)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;
                var sceneName = sceneAt.name;
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                scenes.Add(sceneName);
            }
            
            // Create an AsyncOperationGroup
            var operationGroup = new AsyncOperationGroup(scenes.Count);

            foreach (var scene in scenes)
            {
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                
                operationGroup.operations.Add(operation);
                
                OnSceneUnloaded.Invoke(scene);
            }
            
            // Wait until all AsyncOperations in the group are done

            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }
        }
        
        #endregion
        
        #region naive discrete load
        
        public static async Task NaiveLoadSceneGroup(SceneGroup group)
        {
            foreach (var s in group)
            {
                await SceneManager.LoadSceneAsync(s.Name, LoadSceneMode.Additive);
            }
        }

        public static async Task NaiveUnloadSceneGroup(SceneGroup group)
        {
            foreach (var s in group)
            {
                await SceneManager.UnloadSceneAsync(s.Name);
            }
        }

        #endregion
        
        #endregion
        
        #region fields
        
        private SceneGroup _activeSceneGroup;
        
        #endregion
    }
}
