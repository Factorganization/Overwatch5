using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class Octree
    {
#if UNITY_EDITOR
        #region constructors

        public Octree(Transform parent, Collider[] worldObjs, float minNodeSize, NavGraph navGraph, LayerMask bakeLayer, NavSpaceData navSpaceData, List<NavSpaceBoundsDataHandling> navSpaceBounds)
        {
            _navGraph = navGraph;
            _bakeLayer = bakeLayer;
            _navSpaceData = navSpaceData;
            _navSpaceBounds = navSpaceBounds;
            
            CalculateBounds(parent, worldObjs);
            CreateTree(worldObjs, minNodeSize);
            
            GetEmptyLeaves(_root);
            //ClearNodes();
            GetEdges();

            BakeData();
        }

        #endregion

        #region methodes

        private void CreateTree(Collider[] worldObjs, float minNodeSize)
        {
            _root = new OctreeNode(_bounds, minNodeSize);

            foreach (var obj in worldObjs)
            {
                _root.Divide(obj);
            }
        }
        
        private void CalculateBounds(Transform parent, Collider[] worldObjs)
        {
            _bounds.center = parent.position;
            
            foreach (var worldObj in worldObjs)
                _bounds.Encapsulate(worldObj.bounds);
            
            var size = Vector3.one * Mathf.Max(_bounds.size.x, _bounds.size.y, _bounds.size.z) * 0.6f;
            _bounds.SetMinMax(_bounds.center - size, _bounds.center + size);
        }
        
        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.IsLeaf && node.objs.Count == 0)
            {
                var c = 0;
                
                foreach (var b in _navSpaceBounds)
                {
                    if (!b.Bounds.Contains(node.bounds.center))
                        c++;
                }
                
                if (c >= _navSpaceBounds.Count)
                    return;
                
                _emptyLeaves.Add(node);
                _navGraph.AddNode(node);
                return;
            }
            
            if (node.children is null)
                return;

            foreach (var o in node.children)
                GetEmptyLeaves(o);
        }
        
        private void GetEdges()
        {
            foreach (var el in _emptyLeaves)
            {
                foreach (var ol in _emptyLeaves)
                {
                    var ray = new Ray(el.bounds.center, ol.bounds.center - el.bounds.center);
                    var cast = Physics.Raycast(ray, 10, _bakeLayer);
                    
                    if (cast || Vector3.Distance(el.bounds.center, ol.bounds.center) > 9.9f)
                        continue;
                    
                    _navGraph.AddEdge(el, ol);
                }
            }
        }

        private void BakeData()
        {
            foreach (var n in _navGraph.nodes.Values)
            {
                _tempNodes.Add(new SerializedOctreeNode(n));
            }

            _tempNodes.Sort(CompareSerializedNodes);

            foreach (var e in _navGraph.edges)
            {
                _tempEdges.Add(new SerializedOctreeEdge(e));
            }
            
            _tempEdges.Sort(CompareSerializedEdges);
            
            var i = _tempEdges[^1].depth;

            for (var j = 1; j < i + 1; j++)
            {
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NavSpaceSubData>(), _navSpaceData.subDataPath + "\\sd" + j + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(_navSpaceData.subDataPath + "\\sd" + j + ".asset", ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
                
                var z = AssetDatabase.LoadAssetAtPath(_navSpaceData.subDataPath + "\\sd" + j + ".asset", typeof(NavSpaceSubData)) as NavSpaceSubData;
                _navSpaceData.AddSubData(z);
            }
            
            PopulateBake();
        }

        private void PopulateBake()
        {
            foreach (var n in _tempNodes)
            {
                _navSpaceData.subData[n.depth].AddNode(n);
            }
            
            foreach (var e in _tempEdges)
            {
                _navSpaceData.subData[e.depth].AddEdge(e);
            }
            AssetDatabase.SaveAssets();
        }

        #endregion
        
        #region fields

        private OctreeNode _root;

        private Bounds _bounds;

        private readonly NavGraph _navGraph;
        
        private readonly List<OctreeNode> _emptyLeaves = new();
        
        private readonly LayerMask _bakeLayer;

        private readonly NavSpaceData _navSpaceData;
        
        private int _currentDepthThreshold;
        
        private readonly List<SerializedOctreeNode> _tempNodes = new();
        
        private readonly List<SerializedOctreeEdge> _tempEdges = new();

        private readonly List<NavSpaceBoundsDataHandling> _navSpaceBounds = new();
        
        private static readonly Comparison<SerializedOctreeNode> CompareSerializedNodes =
            (a, b) => (int)Mathf.Sign(a.id - b.id);

        private static readonly Comparison<SerializedOctreeEdge> CompareSerializedEdges =
            (a, b) => (int)Mathf.Sign(a.depth - b.depth);

        #endregion
#endif
    }
}