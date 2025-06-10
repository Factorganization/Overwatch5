using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Inventory
{
    public class InventoryModel
    {
        public ObservableArray<Item> Items { get; set; }
        private InventoryData inventoryData;
        private int capacity;
        
        public event Action<Item[]> OnModelChanged
        {
            add => Items.AnyChange += value;
            remove => Items.AnyChange -= value;
        }
        
        public InventoryModel(IEnumerable<ItemDetails> itemDetails, int capacity)
        {
            this.capacity = capacity;
            Items = new ObservableArray<Item>(capacity);
            foreach (var itemDetail in itemDetails)
            {
                Items.TryAdd(itemDetail.CreateItem(1));
            }
        }

        public void Bind(InventoryData data)
        {
            inventoryData = data;
            inventoryData.capacity = capacity;
            
            bool isNew = inventoryData.Items == null || inventoryData.Items.Length == 0;
            
            if (isNew)
            {
                inventoryData.Items = new Item[capacity];
            }
            /*else {
                for (var i = 0; i < capacity; i++) {
                    if (Items[i] != null) continue;
                    inventoryData.Items[i].details = ItemDatabase.GetDetailsById(Items[i].detailsId);
                }
            }*/
            
            if (isNew && Items.Count != 0) {
                for (var i = 0; i < capacity; i++) {
                    if (Items[i] == null) continue;
                    inventoryData.Items[i] = Items[i];
                }
            }
            
            Items.items = inventoryData.Items;
            Debug.Log($"Bind() called. Inventory has {Items.Count} items before binding.");
        }
        
        // Get the item at the index
        public Item Get(int index) => Items[index];
        
        // Clear the inventory
        public void Clear() => Items.Clear();
        
        // Add the item to the inventory
        public bool Add(Item item)
        {
            bool success = Items.TryAdd(item);
            
            if (success)
            {
                Debug.Log($"Item added! Inventory has {Items.Count} items.");
                
                if (inventoryData != null)
                {
                    inventoryData.Items = Items.items;
                }
            }
            else
            {
                Debug.LogError("Item not added.");
            }
            
            return success;
        } 
        
        // Remove the item from the inventory
        public bool Remove(Item item) => Items.TryRemove(item);
        
        // Swap the items at the source and target indices
        public void Swap(int source, int target) => Items.Swap(source, target);

        // Combine the items at the source and target indices
        public int Combine(int source, int target)
        {
            var total = Items[source].quantity + Items[target].quantity;
            Items[target].quantity = total;
            Remove(Items[source]);
            return total;
        }
    }
}