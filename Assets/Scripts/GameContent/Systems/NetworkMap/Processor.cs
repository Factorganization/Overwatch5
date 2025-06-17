using System;
using FMOD.Studio;
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
    
    private EventInstance _processorEventInstance;
    
    private void Start()
    {
        _alrHacked = false;
        _processorEventInstance = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.ProcessorDestroy);
        _processorEventInstance.start();
    }
    
    public override void OnInteract() 
    {
        if (_alrHacked == false) OnHack();
    }

    protected override void OnHack()
    {
        _alrHacked = true;

        if (_processorType == ProcessorType.Normal)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.ProcessorDestroy, gameObject.transform.position);
            GameManager.Instance.TerminateProcessor();
        }
        
        if (_processorType == ProcessorType.TheOne)
        {
            if (GameManager.Instance.GetNumberOfProcessorsTerminated() < 4)
                return;
            
            GameManager.Instance.TerminateProcessor();
            GameManager.Instance.WinGame();
        }
        
        _processorEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
