using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field : Header("Ambience")]
    [field : Header("Ambient Music")]
    [field : SerializeField] public EventReference Ambient { get; private set; }
    [field : Header("Cinematic")]
    [field : SerializeField] public EventReference IntroCinematic { get; private set; }
    [field : SerializeField] public EventReference IntroCinematicWind { get; private set; }
    
    [field : Header("Context")]
    [field : SerializeField] public EventReference BubbleTubes { get; private set; }
    [field : SerializeField] public EventReference FlapDoor { get; private set; }
    [field : SerializeField] public EventReference PhysicObject { get; private set; }
    [field : SerializeField] public EventReference Rain { get; private set; }
    [field : SerializeField] public EventReference Spark { get; private set; }
    [field : SerializeField] public EventReference Steam { get; private set; }
    [field : SerializeField] public EventReference WaterFall { get; private set; }
    [field : SerializeField] public EventReference WaterFallSmall { get; private set; }
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference CouloirFinal { get; private set; }
    [field: SerializeField] public EventReference Detected { get; private set; }
    [field: SerializeField] public EventReference Menus { get; private set; }
    [field: SerializeField] public EventReference MusicArea { get; private set; }
    [field: SerializeField] public EventReference ProcessorDestroyed { get; private set; }
    
    [field : Header("SFX")]
    [field : Header("Alerte")]
    [field : SerializeField] public EventReference PlayerDetected { get; private set; }
    [field : SerializeField] public EventReference PlayerDetectedByHound { get; private set; }
    [field : SerializeField] public EventReference PlayerSuspected { get; private set; }
    [field : SerializeField] public EventReference SuspiciousAction { get; private set; }
    
    [field : Header("Avatar")]
    [field : SerializeField] public EventReference AvatarDeath { get; private set; }
    [field : SerializeField] public EventReference AvatarFall { get; private set; }
    [field : SerializeField] public EventReference AvatarPickUP { get; private set; }
    [field : SerializeField] public EventReference AvatarRun { get; private set; }
    [field : SerializeField] public EventReference AvatarWalk { get; private set; }
    
    [field : Header("Disjonctor")]
    [field : SerializeField] public EventReference CircuitBreakerAmbient { get; private set; }
    [field : SerializeField] public EventReference CircuitBreakerEndCD { get; private set; }
    [field : SerializeField] public EventReference CircuitBreakerOnCD { get; private set; }
    [field : SerializeField] public EventReference CircuitBreakerTurnON { get; private set; }
    [field : SerializeField] public EventReference CircuitBreakerTurnOFF { get; private set; }
    
    [field : Header("IA")]
    [field : SerializeField] public EventReference CameraRotate { get; private set; }
    [field : SerializeField] public EventReference DroneAmbient { get; private set; }
    [field : SerializeField] public EventReference HoundAmbient { get; private set; }
    [field : SerializeField] public EventReference HoundAttack { get; private set; }
    [field : SerializeField] public EventReference ProcessorAmbient { get; private set; }
    [field : SerializeField] public EventReference ProcessorDestroy { get; private set; }
    
    [field : Header("Items")]
    [field : SerializeField] public EventReference BatteryUsed { get; private set; }
    [field : SerializeField] public EventReference MedkitUsed { get; private set; }
    
    [field : Header("Map")]
    [field : SerializeField] public EventReference MapOpen { get; private set; }
    [field : SerializeField] public EventReference MapClose { get; private set; }
    
    [field : Header("Menus")]
    [field : SerializeField] public EventReference MenuClick { get; private set; }
    [field : SerializeField] public EventReference MenuPauseOpen { get; private set; }
    [field : SerializeField] public EventReference MenuPauseClose { get; private set; }
    [field : SerializeField] public EventReference MenuStart { get; private set; }
    
    [field : Header("Sabotage")]
    [field : SerializeField] public EventReference HackingBlocked { get; private set; }
    [field : SerializeField] public EventReference HackingClickBox { get; private set; }
    [field : SerializeField] public EventReference HackingConfirmed { get; private set; }
    [field : SerializeField] public EventReference HackingUnblocked { get; private set; }
    
    [field : Header("Scan")]
    [field : SerializeField] public EventReference Scan { get; private set; }
    [field : SerializeField] public EventReference ScanJunctionSuccess { get; private set; }
    [field : SerializeField] public EventReference ScanPeriphSuccess { get; private set; }
    
    [field : Header("SelectionWheel")]
    [field : SerializeField] public EventReference SelectionWheelOpen { get; private set; }
    [field : SerializeField] public EventReference SelectionWheelClose { get; private set; }
    
    [field : Header("Tool battery")]
    [field : SerializeField] public EventReference ToolBootUP { get; private set; }
    [field : SerializeField] public EventReference ToolOff { get; private set; }
    [field : SerializeField] public EventReference ToolShutDown { get; private set; }
    
    [field : Header("Snapshots")]
    [field : SerializeField] public EventReference EffectGamePaused { get; private set; }
    [field : SerializeField] public EventReference EffectEcho { get; private set; }
    
    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of FMODEvents detected. Destroying the new instance.");
            return;
        }
        
        Instance = this;
    }
}
