using GameContent.Management;
using Systems.Inventory.Interface;
using UnityEngine;

public class HackableJunction : MonoBehaviour, IInteractible
{
    [SerializeField] protected RoomMap map;
    [SerializeField] protected float _hackingTime;
    [SerializeField] protected string _interactibleName;
    [SerializeField] protected float _suspicionAmount;
        
    public float HackingTime => _hackingTime;
    public string InteractibleName => _interactibleName;
    public bool _alrHacked;
    
    
    private void Start()
    {
        _alrHacked = false;
    }

    public virtual void OnInteract() 
    {
        if (_alrHacked == false) OnHack();
    }
    
    protected virtual void OnHack()
    {
        _alrHacked = true;
        NetworkMapController.Instance.RevealRoom(map);
        SuspicionManager.Manager.AddSuspicion(_suspicionAmount);
    }
}