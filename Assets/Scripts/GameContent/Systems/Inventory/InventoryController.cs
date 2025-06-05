using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Inventory {
    public class ViewModel {
        public readonly int Capacity;

        public ViewModel(InventoryModel model, int capacity) {
            Capacity = capacity;
        }
    }
    
    public class InventoryController { 
        readonly InventoryView view;
        readonly InventoryModel model;
        readonly int capacity;
        
        public InventoryModel Model => model;

        InventoryController(InventoryModel model, int capacity) {
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(capacity > 0, "Capacity is less than 1");
            this.model = model;
            this.capacity = capacity;
        }
        
        public void Bind(InventoryData data) => model.Bind(data);

        IEnumerator Initialize() {
            yield return view.InitView(new ViewModel(model, capacity));
            
            view.OnDrop += HandleDrop;
            model.OnModelChanged += HandleModelChanged;

            //RefreshView();
        }

        void HandleDrop(Slot originalSlot, Slot closestSlot) {
            // Moving to Same Slot or Empty Slot
            if (originalSlot.Index == closestSlot.Index || closestSlot.ItemId.Equals(SerializableGuid.Empty)) {
                model.Swap(originalSlot.Index, closestSlot.Index);
                return;
            }
            
            // Moving to Non-Empty Slot
            var sourceItemId = model.Get(originalSlot.Index).details.id;
            var targetItemId = model.Get(closestSlot.Index).details.id;
                        
            if (sourceItemId.Equals(targetItemId) && model.Get(closestSlot.Index).details.maxStack > 1) { 
                model.Combine(originalSlot.Index, closestSlot.Index);
            } else {
                model.Swap(originalSlot.Index, closestSlot.Index);
            }
        }

        void HandleModelChanged(IList<Item> items) => RefreshView();
        
        void RefreshView() {
            for (int i = 0; i < capacity; i++) {
                var item = model.Get(i);
                if (item == null || item.id.Equals(SerializableGuid.Empty)) {
                    view.Slots[i].Set(SerializableGuid.Empty, null);
                } else {
                    view.Slots[i].Set(item.id, item.details.icon, item.quantity);
                }
            }
        }
        
        public void AddItem(ItemDetails itemDetails, int quantity) {
            foreach (var item in model.Items.items) 
            {
                if (item.details != itemDetails) continue;
                
                if (item.details.name.Equals(itemDetails.name)) {
                    Debug.Log(quantity);
                    item.quantity += quantity;
                    Inventory.Instance.RadialMenu.RefreshVisual();
                    return;
                }
                
                var newItem = itemDetails.CreateItem(quantity);
                model.Add(newItem);
                return;
            }
        }
        
        public void SubtractItem(ItemDetails itemDetails, int quantity) {
            foreach (var item in model.Items.items) 
            {
                if (item.details != itemDetails) continue;
                
                if (item.details.name.Equals(itemDetails.name)) {
                    if (item.quantity <= 0) return;
                    Debug.Log(quantity);
                    item.quantity -= quantity;
                    Inventory.Instance.RadialMenu.RefreshVisual();
                    return;
                }
            }
        }
        
        #region Builder
        
        public class Builder {
            InventoryView view;
            IEnumerable<ItemDetails> itemDetails;
            int capacity;

            public Builder WithStartingItems(IEnumerable<ItemDetails> itemDetails) {
                this.itemDetails = itemDetails;
                return this;
            }

            public Builder WithCapacity(int capacity) {
                this.capacity = capacity;
                return this;
            }

            public InventoryController Build() {
                InventoryModel model = itemDetails != null 
                    ? new InventoryModel(itemDetails, capacity) 
                    : new InventoryModel(Array.Empty<ItemDetails>(), capacity);

                return new InventoryController(model, capacity);
            }
        }
        
        #endregion Builder
    }
}