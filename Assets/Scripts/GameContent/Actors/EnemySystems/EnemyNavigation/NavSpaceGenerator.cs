using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class NavSpaceGenerator : MonoBehaviour
    {
        #region properties

        public List<NavSpaceBoundsDataHandling> Bounds => bounds;

        #endregion
        
        #region methodes
#if UNITY_EDITOR
        public void Bake()
        {
            navSpaceData.subData = new List<NavSpaceSubData>();
            navSpaceData.minBoundSize = minNodeSize;
            
            var tp = AssetDatabase.GetAssetPath(navSpaceData);
            var s = tp.Split('/');
            var ns = "";
            for (var i = 0; i < s.Length - 1; i++)
            {
                ns += s[i];
                ns += '\\';
            }
            var sdp = ns + navSpaceData.name + "Subs";
            
            if (Directory.Exists(sdp))
                Directory.Delete(sdp, true);
            AssetDatabase.Refresh();
            Directory.CreateDirectory(sdp);
            navSpaceData.subDataPath = sdp;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NavSpaceSubData>(), sdp + "\\sd0" + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.ImportAsset(sdp + "\\sd0" + ".asset", ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            
            var z = AssetDatabase.LoadAssetAtPath(sdp + "\\sd0" + ".asset", typeof(NavSpaceSubData)) as NavSpaceSubData;
            navSpaceData.AddSubData(z);
            
            _navGraph = new NavGraph();
            _octree = new Octree(transform, worldObjs, minNodeSize, _navGraph, bakeBlockingLayer, navSpaceData, bounds);
        }

        public void AddBoundary()
        {
            var b = new GameObject("Bounds" + bounds.Count, typeof(NavSpaceBounds), typeof(NavSpaceBoundsDataHandling));
            b.transform.SetParent(transform);
            b.transform.position = transform.position + Vector3.down * (bounds.Count + 1);
            
            var dh = b.GetComponent<NavSpaceBoundsDataHandling>();
            dh.Bounds = new Bounds(Vector3.zero, Vector3.one * 5.0f);
            dh.Position = transform.position + Vector3.up * (bounds.Count + 1);
            dh.navSpaceGeneratorRef = this;
            
            bounds.Add(dh);
        }
#endif
        #endregion
        
        #region fields
        
        [SerializeField] private NavSpaceData navSpaceData;
        
        [SerializeField] private Collider[] worldObjs;
        
        [SerializeField] private List<NavSpaceBoundsDataHandling> bounds;
        
        [SerializeField] private float minNodeSize;
        
        [SerializeField] private LayerMask bakeBlockingLayer;
        
        private NavGraph _navGraph;
        
        private Octree _octree;
        
        #endregion
    }
}