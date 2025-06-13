using System;
using GameContent.Management;
using UnityEngine;

enum ProcessorType
{
    Normal,
    TheOne
}

public class Processor : HackableJunction
{
    [SerializeField] private ProcessorType _processorType;
    
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

        if (_processorType == ProcessorType.Normal)
        {
            GameManager.Instance.TerminateProcessor();
        }
        
        if (_processorType == ProcessorType.TheOne && GameManager.Instance.GetNumberOfProcessorsTerminated() > 4)
        {
            GameManager.Instance.WinGame();
        }
    }
}
