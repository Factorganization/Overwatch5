namespace Systems.Inventory.Interface
{
    public interface IInteractible
    {
        public string InteractibleName { get; }

        public virtual void OnInteract() { }
    }
}
