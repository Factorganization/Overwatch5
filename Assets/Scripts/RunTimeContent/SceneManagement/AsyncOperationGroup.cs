using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RunTimeContent.SceneManagement
{
    public readonly struct AsyncOperationGroup
    {
        #region properties

        public float Progress => operations.Count == 0 ? 0 : operations.Average(o => o.progress);
        
        public bool IsDone => operations.All(o => o.isDone);
        
        #endregion
        
        #region methodes
        
        public AsyncOperationGroup(int initialCapacity)
        {
            operations = new List<AsyncOperation>(initialCapacity);
        }
        
        #endregion
        
        #region fields

        public readonly List<AsyncOperation> operations;

        #endregion
    }
}