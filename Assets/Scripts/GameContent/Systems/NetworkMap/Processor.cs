using System;
using UnityEngine;

enum ProcessorType
{
    Normal,
    TheOne
}

public class Processor : HackableJunction
{
    [SerializeField] private RoomMap _roomMap;
    
    private void Start()
    {
        _alrHacked = false;
    }
    
    public new void OnInteract() 
    {
        if (_alrHacked == false) OnHack();
    }

    protected override void OnHack()
    {
        _alrHacked = true;
       
    }
}
