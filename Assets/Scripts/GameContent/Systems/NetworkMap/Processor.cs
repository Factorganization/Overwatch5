using System;
using UnityEngine;

public class Processor : HackableJunction
{
    public static Processor Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        
        Debug.Log("Processor hacked");
    }
}
