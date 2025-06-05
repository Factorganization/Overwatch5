using System.Collections;

namespace Systems.Inventory
{
    public class InventoryView : StorageView
    {
        //[SerializeField] string panelName = "Inventory";
        
        public override IEnumerator InitView(ViewModel viewModel)
        {
            Slots = new Slot[viewModel.Capacity];
            root = document.rootVisualElement;
            root.Clear();
            
            root.styleSheets.Add(styleSheet);

            container = root.CreateChild("container");
            
            var inventory = container.CreateChild("inventory");
            inventory.CreateChild("inventoryFrame");
            
            var slotsContainer = inventory.CreateChild("slotsContainer");
            for (int i = 0; i < viewModel.Capacity; i++)
            {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                Slots[i] = slot;
            }
            
            ghostIcon = container.CreateChild("ghostIcon");
            ghostIcon.BringToFront();

            yield return null;
        }
    }
}
