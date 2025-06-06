using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems.Inventory
{
    public interface IObservableArray<T>
    {
        event Action<T[]> AnyChange;
        
        int Count { get; }
        T this[int index] { get; }
        
        void Swap(int indexA, int indexB);
        void Clear();
        bool TryAdd(T item);
        bool TryAddAt(int index, T item);
        bool TryRemove(T item);
        bool TryRemoveAt(int index);
    }

    [Serializable]
    public class ObservableArray<T> : IObservableArray<T>
    {
        public T[] items;

        public event Action<T[]> AnyChange = delegate { };
        public int Count => items.Count(i => i != null);
        public int Length => items.Length;
        public T this[int index] => items[index];

        public ObservableArray(int size = 21, IList<T> initialList = null)
        {
            items = new T[size];
            if (initialList != null)
            {
                initialList.Take(size).ToArray().CopyTo(items, 0);
                Invoke();
            }
        }

        void Invoke() => AnyChange.Invoke(items);

        public void Swap(int indexA, int indexB)
        {
            (items[indexA], items[indexB]) = (items[indexB], items[indexA]);
            Invoke();
        }

        public void Clear()
        {
            items = new T[items.Length];
            Invoke();
        }

        public bool TryAdd(T item)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (TryAddAt(i, item)) return true;
            }

            return false;
        }

        public bool TryAddAt(int index, T item)
        {
            if (index < 0 || index >= items.Length) return false;

            if (items[index] != null) return false;

            items[index] = item;
            Invoke();
            return true;
        }

        public bool TryRemove(T item)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (EqualityComparer<T>.Default.Equals(items[i], item) && TryRemoveAt(i)) return true;
            }

            return false;
        }

        public bool TryRemoveAt(int index)
        {
            if (index < 0 || index >= items.Length) return false;

            if (items[index] == null) return false;

            items[index] = default;
            Invoke();
            return true;
        }
    }
}