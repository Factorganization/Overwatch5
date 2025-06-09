using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace RunTimeContent.SceneManagement
{
    [Serializable]
    public class SceneGroup : IEnumerable<SceneData>
    {
        #region methodes
        
        public string FindSceneNameByType(SceneType sceneType)
        {
            return scenes.FirstOrDefault(scene => scene.sceneType == sceneType)?.reference.Name;
        }

        public IEnumerator<SceneData> GetEnumerator() => scenes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion
        
        #region fields
        
        public string groupName;
        
        public List<SceneData> scenes;
        
        #endregion
    }
    
    [Serializable]
    public class SceneData
    {
        #region properties
        
        public string Name => reference.Name;
        
        #endregion
        
        #region fields
        
        public SceneReference reference;
        
        public SceneType sceneType;
        
        #endregion
    }
    
    public enum SceneType
    {
        ActiveScene,
        MainMenu,
        UserInterface,
        HUD,
        Cinematic,
        Environment,
        Tooling
    }
}