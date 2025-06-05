using System;
using System.Collections;
using System.Collections.Generic;
using GameContent.Actors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameContent.Management
{
    public class Pool<T> : IEnumerable<T> where T : Actor
    {
        #region properties

        public Queue<T> ActorQueue { get; private set; }

        public int Count => ActorQueue.Count;
        
        public Type Type => typeof(T);
        
        #endregion

        #region constructors

        public Pool(PoolData<T> poolData, Transform parentTransform = null)
        {
            _actorPrefab = poolData.actorPrefab;
            _poolCount = poolData.poolCount;
            _pooledPosition = poolData.pooledPosition;
            _poolParent = parentTransform;

            Init();
        }

        #endregion
        
        #region methodes

        private void Init()
        {
            ActorQueue = new Queue<T>();

            for (var i = 0; i < _poolCount; i++)
            {
                var o = Object.Instantiate(_actorPrefab, _pooledPosition, Quaternion.identity, _poolParent);
                ActorQueue.Enqueue(o);
            }
        }
        
        public T Peek() => ActorQueue.Peek();
        
        public void Enqueue(T actor) => ActorQueue.Enqueue(actor);
        
        public T Dequeue() => ActorQueue.Dequeue();

        public T poolActor()
        {
            var a = Dequeue();
            Enqueue(a);
            
            return a;
        }
        
        public IEnumerator<T> GetEnumerator() => ActorQueue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion

        #region fields

        private readonly T _actorPrefab;
        
        private readonly int _poolCount;
        
        private readonly Vector3 _pooledPosition;
        
        private readonly Transform _poolParent;

        #endregion
    }

    [Serializable]
    public struct PoolData<T> where T : Actor
    {
        #region constructors
        
        public PoolData(T actorPrefab, int poolCount, Vector3 pooledPosition)
        {
            this.actorPrefab = actorPrefab;
            this.poolCount = poolCount;
            this.pooledPosition = pooledPosition;
        }
        
        #endregion
        
        #region fields
        
        public T actorPrefab;
        
        public int poolCount;
        
        public Vector3 pooledPosition;
        
        #endregion
    }
}