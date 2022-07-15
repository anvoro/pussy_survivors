
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tank.Helpers.ObjectPool
{
    public sealed class MultiKeyObjectPool<TK, TV> where TK : UnityEngine.Object
    {
        private readonly Dictionary<TK, ObjectPool<TV>> _pools = new Dictionary<TK, ObjectPool<TV>>();
        private readonly Func<TK, TV> _factory;
        private readonly Action<TV> _resetMethod;
        private readonly Action<GameObject, int> _setNameIndex;

        public MultiKeyObjectPool(Func<TK, TV> factory, Action<TV> resetMethod = null, Action<GameObject, int>  setNameIndex = null)
        {
            this._factory = factory;
            this._resetMethod = resetMethod;
            this._setNameIndex = setNameIndex;
        }

        public void Create(TK key, int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(count)} must be > 0");

            this.CheckContainsKey(key);

            this._pools[key].Create(count);
        }

        public TV GetOrCreate(TK key)
        {
            this.CheckContainsKey(key);

            return this._pools[key].GetOrCreate();
        }

        private void CheckContainsKey(TK key)
        {
            if (this._pools.ContainsKey(key) == true)
                return;

            ObjectPool<TV> pool = new ObjectPool<TV>(() => this._factory.Invoke(key), this._resetMethod, this._setNameIndex);
            this._pools.Add(key, pool);
        }

        public void Return(TK key, TV value)
        {
            this._pools[key].Return(value);
        }
    }
}
