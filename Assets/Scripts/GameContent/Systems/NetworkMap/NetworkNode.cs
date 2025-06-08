using System.Collections.Generic;
using GameContent.Actors;
using TMPro;
using UnityEngine;

public enum NodeType { Junction, Device, Processor }

public class NetworkNode : MonoBehaviour
{
    public string nodeId;
    public NodeType type;
    public GameObject nodeVisual;
    public Actor actor;
    public TextMeshProUGUI name;
    public List<NetworkNode> _originalConnectedNodes = new List<NetworkNode>();
    public List<NetworkNode> _connectedNodes = new List<NetworkNode>();
    public bool hidden;

    private void Start()
    {
        if (hidden)
        {
            name.text = "???";
        }
        else
        {
            name.text = nodeId;
            _originalConnectedNodes = new List<NetworkNode>(_connectedNodes);
        }
    }
}