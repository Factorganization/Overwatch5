using System;
using UnityEngine;

namespace Systems.Inventory
{
    [Serializable]
    public class Item
    {
        [field: SerializeField] public SerializableGuid id;
        [field: SerializeField] public SerializableGuid detailsId; 
        public ItemDetails details;
        public int quantity;
        
        public Item(ItemDetails details, int quantity)
        {
            id = SerializableGuid.NewGuid();
            detailsId = details.id;
            this.details = details;
            this.quantity = quantity;
        }
    }
}
