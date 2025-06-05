using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class OctreeNode
    {
        #region properties

        public bool IsLeaf => children is null;

        #endregion

        #region constructors

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            this.bounds = bounds; 
            _minNodeSize = minNodeSize;

            var newSize = bounds.size * 0.5f;
            var centerOffset = bounds.size * 0.25f;
            var parentCenter = bounds.center;

            for (var i = 0; i < 8; i++)
            {
                var childCenter = parentCenter;
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);
                _childBounds[i] = new Bounds(childCenter, newSize);
            }
        }

        #endregion
        
        #region methodes

        public void Divide(Collider obj) => Divide(new OctreeObj(obj));

        private void Divide(OctreeObj obj)
        {
            if (bounds.size.x <= _minNodeSize)
            {
                objs.Add(obj);
                return;
            }

            children ??= new OctreeNode[8];
            var intersectChild = false;

            for (var i = 0; i < 8; i++)
            {
                children[i] ??= new OctreeNode(_childBounds[i], _minNodeSize);

                if (!obj.Intersects(_childBounds[i]))
                    continue;
                
                children[i].Divide(obj);
                intersectChild = true;
            }
            
            if (intersectChild)
                objs.Add(obj);
        }

        #endregion
        
        #region fields

        public readonly List<OctreeObj> objs = new();

        public Bounds bounds;
        
        private readonly Bounds[] _childBounds = new Bounds[8];

        public OctreeNode[] children;

        private readonly float _minNodeSize;

        #endregion
    }
}