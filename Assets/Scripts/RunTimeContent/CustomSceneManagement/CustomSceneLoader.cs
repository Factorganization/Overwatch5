using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
namespace RunTimeContent.CustomSceneManagement
{
    public static class CustomSceneLoader
    {
        public static void LoadSceneGroup(string groupName, SceneListSo list)
        {
            CustomSceneGroup sg = null;
            foreach (var g in list.sceneGroups)
            {
                if (g.sceneGroupName != groupName)
                    continue;
                
                sg = g;
                break;
            }

            if (sg is null)
            {
                Debug.LogWarning($"Scene Group ' {groupName} ' not found!");
                return;
            }
            
            if (sg.scenePaths.Length == 0)
            {
                Debug.LogWarning($"Scene Group ' {groupName} ' is empty!");
                return;
            }
            
            SceneManager.LoadSceneAsync(sg.scenePaths[0].scene.name, LoadSceneMode.Single);

            for (var i = 1; i < sg.scenePaths.Length; i++)
                SceneManager.LoadSceneAsync(sg.scenePaths[i].scene.name, LoadSceneMode.Additive);
        }
    }
}

#endif