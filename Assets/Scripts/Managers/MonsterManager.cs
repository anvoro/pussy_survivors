using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Game.SpawnShapes;
using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterManager : MonoBehaviour
	{
		[SerializeField]
		private MonsterManager _monsterManager;

		[SerializeField]
		private MonsterController[] _monsterPrefabs;

		[SerializeField]
		private MonstersPool _pool;
		
		public CompositeSpawnShape SpawnShape;

		public Transform MonsterParent;
		
		private List<MonsterController> _activeMonsters = new List<MonsterController>();
		
		private static MonsterManager monsterManager;

		//todo добавить фильтрацию по IsDead
		public List<MonsterController> ActiveMonsters => this._activeMonsters.Where(e => e.IsDead == false).ToList();
		
		public static MonsterManager Instance => monsterManager;

		private void Awake()
		{
			monsterManager = this._monsterManager;

			foreach (MonsterController prefab in this._monsterPrefabs)
			{
				this._pool.Create(prefab, 10);
			}

			currentSpawnDelay = this.spawnDelay;
			Spawn(5);
		}

		public float spawnDelay;
		private float currentSpawnDelay;

		public int maxMonstersAlive;
		private int monstersAlive;

		private void Update()
		{
			if (this.currentSpawnDelay > 0)
			{
				this.currentSpawnDelay -= Time.deltaTime;
				return;
			}

			if (currentSpawnDelay <= 0)
			{
				Spawn(maxMonstersAlive);
				this.currentSpawnDelay = spawnDelay;
			}
		}

		private void Spawn(int maxMonsters)
		{
			
			
			foreach (MonsterController prefab in this._monsterPrefabs)
			{
				if(monstersAlive >= maxMonsters)
					break;
				
				for (int i = 0; i <= maxMonsters / this._monsterPrefabs.Length; i++)
				{
					if(monstersAlive >= maxMonsters)
						break;
					
					MonsterController monster = this._pool.GetOrCreate(prefab);
					monster.OnCharacterDie -= OnMonsterDeath;
					monster.OnCharacterDie += OnMonsterDeath;

					monster.gameObject.SetActive(true);
					
					monster.transform.position = this.SpawnShape.GetRandomPoint();
				
					monster.IsDead = false;
				
					this._activeMonsters.Add(monster);

					monstersAlive++;
				}
			}
		}

		private void OnMonsterDeath(CharacterBase monster)
		{
			monster.IsDead = true;

			this._pool.Return((MonsterController)monster);
			
			monstersAlive--;
		}
	}
}