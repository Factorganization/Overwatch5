using System.Collections.Generic;
using UnityEngine;

public class RoomMap : MonoBehaviour
{
    [SerializeField] List<NetworkNode> _nodes = new List<NetworkNode>();
    [SerializeField] List<MapLink> _mapLink = new List<MapLink>();
    
    public List<NetworkNode> Nodes => _nodes;
    public List<MapLink> MapLink => _mapLink;

    private void Start()
    {
        foreach(MapLink mapLink in _mapLink)
        {
            mapLink.RoomMap = this;
        }
    }
}
