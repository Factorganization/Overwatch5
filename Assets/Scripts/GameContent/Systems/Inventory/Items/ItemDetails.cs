using UnityEngine;

namespace Systems.Inventory
{
    public enum Type
    {
        MultiTool,
        Consumable,
    }

    public enum Action
    {
        None,
        Heal,
        Recharge,
    }
    
    [CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/ItemDetails")]
    public class ItemDetails : ScriptableObject
    {
        public string Name;
        public Type type;
        public Action action;
        public int maxStack;
        public SerializableGuid id = SerializableGuid.NewGuid();
        public Sprite icon, chosenIcon;
        
        private void AssignNewGuid()
        {
            id = SerializableGuid.NewGuid();
        }
        
        [TextArea] public string description;

        public Item CreateItem(int quantity)
        {
            return new Item(this, quantity);
        }

        public virtual void OnAction()
        {
            switch (action)
            {
                case Action.None:
                    break;
                case Action.Heal:
                    if (Inventory.Instance.Controller.Model.Items[1].quantity <= 0)
                    {
                        GameUIManager.Instance.ShowNotification("No medkits left!");
                        return;
                    }
                    if (Hero.Instance.Health.CurrentHealth >= Hero.Instance.Health.MaxHealth)
                    {
                        GameUIManager.Instance.ShowNotification("You are already at full health!");
                        return;
                    }
                    
                    Hero.Instance.Health.Heal(25);
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    break;
                case Action.Recharge:
                    if (Inventory.Instance.Controller.Model.Items[2].quantity <= 0)
                    {
                        GameUIManager.Instance.ShowNotification("No batteries left!");
                        return;
                    }
                    if (Hero.Instance.MultiToolObject.CurrentBattery >= Hero.Instance.MultiToolObject.MaxBattery)
                    {
                        GameUIManager.Instance.ShowNotification("MultiTool is already fully charged!");
                        return;
                    }
                    
                    Hero.Instance.MultiToolObject.RechargeBattery(25);
                    Inventory.Instance.Controller.SubtractItem(this,1);
                    break;
                default:
                    break;
            }
        }
    }
}
