using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

			this._pool.Init();
			
			foreach (MonsterController prefab in this._monsterPrefabs)
			{
				this._pool.Create(prefab, this.maxMonstersAlive / this._monsterPrefabs.Length);
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
			foreach (MonsterController prefab in this._monsterPrefabs.OrderBy(a => Guid.NewGuid()).ToArray())
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

		private async void OnMonsterDeath(CharacterBase character)
		{
			var monster = (MonsterController)character;

			GameManager.Instance.Player.CurrentXP += monster.KillXP;
			
			monster.IsDead = true;
			monstersAlive--;
			monster.Collider2D.enabled = false;
			
			float value = 1f;
			while (value > 0)
			{
				monster.SetDissolvePower(value);

				value -= 0.02f;
					
				await Task.Yield();
			}
			
			this._pool.Return(monster);
		}
	}
}