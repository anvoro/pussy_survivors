
using Tank.Game.SpawnShapes;
using Tank.Helpers.ObjectPool;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    [DisallowMultipleComponent]
    internal class MonstersPool : MonoBehaviour
    {
        private MultiKeyObjectPool<MonsterController, MonsterController> _pool;
        
        private Transform _unitsParent;
        
        public void Init()
        {
            this._pool = new MultiKeyObjectPool<MonsterController, MonsterController>(this.CreateMonster, this.ResetMonster, this.SetNameIndex);
        }
        
        public void Create(MonsterController prefab, int count)
        {
            this._pool.Create(prefab, count);
        }
        
        public MonsterController GetOrCreate(MonsterController prefab)
        {
            return this._pool.GetOrCreate(prefab);
        }
        
        public void Return(MonsterController monster)
        {
            this._pool.Return(monster.Prefab, monster);
        }
        
        private void ResetMonster(MonsterController monster)
        {
            monster.Reset();
            monster.gameObject.SetActive(false);
        }
        
        private MonsterController CreateMonster(MonsterController prefab)
        {
            MonsterController instance = Instantiate(prefab, MonsterManager.Instance.MonsterParent);
            instance.Prefab = prefab;
        
            return instance;
        }

        private void SetNameIndex(GameObject go, int index)
        {
            go.name = string.Concat(go.name, $"_{index}");
        }
    }
}
