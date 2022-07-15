
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tank.Helpers.ObjectPool
{
    public sealed class ObjectPool<T>
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly Func<T> _factory;
        private readonly Action<T> _resetMethod;
        private readonly Action<GameObject, int> _setNameIndex;

        public int Count { get; private set; }

        public ObjectPool(Func<T> factory, Action<T> resetMethod = null, Action<GameObject, int> setNameIndex = null)
        {
            this._factory = factory;
            this._resetMethod = resetMethod;
            this._setNameIndex = setNameIndex;

            this.Count = 0;
        }

        public void Create(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(count)} must be >= 0");

            for (int i = 0; i < count; i++)
            {
                T item = this._factory();

                if (this._setNameIndex != null)
                {
                    if(item is GameObject go)
                        this._setNameIndex(go, this.Count);
                    else if(item is MonoBehaviour mb)
                        this._setNameIndex(mb.gameObject, this.Count);
                }

                this.Return(item);

                this.Count++;
            }
        }

        public T GetOrCreate()
        {
            if (this._pool.Count == 0)
            {
                this.Create(1);
            }

            return this._pool.Pop();
        }

        public void Return(T item)
        {
            this._resetMethod?.Invoke(item);
            this._pool.Push(item);
        }
    }
}
