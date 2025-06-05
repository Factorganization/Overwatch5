using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RunTimeContent.CustomSceneManagement
{
    [CreateAssetMenu(menuName = "Custom SceneManagement/Scene List", fileName = "Scene List")]
    public class SceneListSo : ScriptableObject
    {
        #region methodes
#if UNITY_EDITOR
        public void LoadSceneGroup()
        {
            CustomSceneGroup sg = null;
            foreach (var g in sceneGroups)
            {
                if (g.sceneGroupName != currentSceneGroupName)
                    continue;
                
                sg = g;
                break;
            }

            if (sg is null)
            {
                Debug.LogWarning($"Scene Group ' {currentSceneGroupName} ' not found!");
                return;
            }

            if (sg.scenePaths.Length == 0)
            {
                Debug.LogWarning($"Scene Group ' {currentSceneGroupName} ' is empty!");
                return;
            }
            
            EditorSceneManager.OpenScene($"Assets/Scenes/{sg.scenePaths[0].additionalScenePath}{sg.scenePaths[0].scene.name}.unity", OpenSceneMode.Single);

            for (var i = 1; i < sg.scenePaths.Length; i++)
                EditorSceneManager.OpenScene($"Assets/Scenes/{sg.scenePaths[i].additionalScenePath}{sg.scenePaths[i].scene.name}.unity", OpenSceneMode.Additive);
        }
#endif

        #endregion
        
        #region fields
        
        public CustomSceneGroup[] sceneGroups;

        public string currentSceneGroupName;
        
        #endregion
    }

    [System.Serializable]
    public class SceneDataPath
    {
        #region fields
        
        public SceneAsset scene;
        
        public string additionalScenePath;

        #endregion
    }
    
    [System.Serializable]
    public class CustomSceneGroup
    {
        #region fields

        public string sceneGroupName;

        public SceneDataPath[] scenePaths;
        
        #endregion
    }
}
#endif