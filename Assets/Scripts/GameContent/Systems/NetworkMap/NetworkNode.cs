using System;
using GameContent.Actors;
using GameContent.Actors.EnemySystems.Seekers;
using TMPro;
using UnityEngine;

public enum NodeType { Junction, Device, Processor }

public class NetworkNode : MonoBehaviour
{
    public bool hidden;
    public string nodeId;
    public NodeType type;
    public EnemyCamera actor;
    public TextMeshProUGUI name;
    
    public EnemyCamera OriginalActor => originalActor;
    
    [Header("Private Variables")]
    [SerializeField] private GameObject nodeVisual;
    [SerializeField]private EnemyCamera originalActor;

    private void Awake()
    {
        if (actor != null)
        {
            originalActor = actor;
        }
        else
        {
            originalActor = null;
        }
    }

    private void Start()
    {
        name.text = hidden ? "???" : nodeId;
    }
}