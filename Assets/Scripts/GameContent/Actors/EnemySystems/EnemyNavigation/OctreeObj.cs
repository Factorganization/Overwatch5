using UnityEngine;

namespace GameContent.Actors.EnemySystems.EnemyNavigation
{
    public class OctreeObj
    {
        #region constructors

        public OctreeObj(Collider obj)
        {
            _bounds = obj.bounds;
        }

        #endregion

        #region methodes

        public bool Intersects(Bounds other) => _bounds.Intersects(other);

        #endregion
        
        #region fields

        private Bounds _bounds;

        #endregion
    }
}