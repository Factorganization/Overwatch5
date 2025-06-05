using UnityEngine;

public class Processor : HackableJunction
{
    
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
