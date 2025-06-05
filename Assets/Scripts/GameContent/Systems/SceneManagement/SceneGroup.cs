using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace Systems.SceneManagement
{
    [Serializable]
    public class SceneGroup
    {
        public string GroupName;
        public List<ScenaData> Scenes;
        
        public string FindSceneNameByType(SceneType sceneType)
        {
            return Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
        }
    }
    
    [Serializable]
    public class ScenaData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;
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