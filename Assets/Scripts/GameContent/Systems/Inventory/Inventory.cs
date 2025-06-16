using System.Collections.Generic;
using Systems.Persistence;
using UnityEngine;

namespace Systems.Inventory {
    public class Inventory : MonoBehaviour, IBind<InventoryData>
    {
        public static Inventory Instance;
        
        [SerializeField] int capacity;
        [SerializeField] List<ItemDetails> startingItems = new List<ItemDetails>();
        [SerializeField] private RadialMenu radialMenu;
        
        [field: SerializeField] public SerializableGuid Id { get; set; }
        
        public RadialMenu RadialMenu => radialMenu;
        
        InventoryController controller;
        
        public InventoryController Controller => controller;
        
        private void Awake()
        {
            controller = new InventoryController.Builder()
                .WithStartingItems(startingItems)
                .WithCapacity(capacity)
                .Build();
            Instance = this;
        }

        public void Bind(InventoryData data)
        {
            controller.Bind(data); 
            data.Id = Id; 
        }
        
        public void EquipItem(ItemDetails item)
        {
            Hero.Instance.CurrentEquipedItem = item;

            GameUIManager.Instance.CrossHair.SetActive(Hero.Instance.CurrentEquipedItem.type == Type.MultiTool);
        }
    }
}