using Systems.Inventory;
using Systems.Inventory.Interface;
using UnityEngine;

namespace Systems.TakeResources
{
    public class WorldItem : MonoBehaviour, IInteractible
    {
        public ItemDetails itemDetails;
        public int quantity;

        public string InteractibleName => itemDetails.Name;

        public void OnInteract()
        {
            Inventory.Inventory.Instance.Controller.AddItem(itemDetails, quantity);
            Destroy(gameObject);
        }
    }
}
