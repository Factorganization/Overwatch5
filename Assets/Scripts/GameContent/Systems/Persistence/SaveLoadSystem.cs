using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.Persistence
{
    [Serializable] public class GameData
    {
        public string Name;
        public string CurrentLevelName;
        public PlayerData playerData;
        public InventoryData inventoryData;
    }

    public interface ISaveable
    {
        SerializableGuid Id { get; set; }
    }
    
    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid Id { get; set; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        [SerializeField] public GameData gameData;
        
        IDataService dataService;
        
        protected override void Awake()
        {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        //private void Start() => NewGame();

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Menu") return;
            
            Bind<Hero, PlayerData>(gameData.playerData);
            Bind<Inventory.Inventory, InventoryData>(gameData.inventoryData);
        }

        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data == null)
                {
                    data = new TData {Id = entity.Id};
                }
                entity.Bind(data);
            }
        }
        
        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id == entity.Id);
                if (data == null)
                {
                    data = new TData {Id = entity.Id};
                    datas.Add(data);
                }
                entity.Bind(data);
            }
        }
        
        public void NewGame()
        {
            gameData = new GameData
            {
                Name = "New Game",
                CurrentLevelName = "OutdoorsScene",
            };
            SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void SaveGame()
        {
            Debug.Log("Saving game...");
            dataService.Save(gameData);
        } 

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);

            if (String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
            {
                gameData.CurrentLevelName = "TestInventory";
            }
            
            SceneManager.LoadScene(gameData.CurrentLevelName);
        }
        
        public void ReloadGame() => LoadGame(gameData.Name);
        
        public void DeleteGame(string gameName) => dataService.Delete(gameName);
        
        public void DeleteAllGames() => dataService.DeleteAll();
        
        public void ListSaves()
        {
            foreach (var save in dataService.ListSaves())
            {
                Debug.Log(save);
            }
        }
    }
}