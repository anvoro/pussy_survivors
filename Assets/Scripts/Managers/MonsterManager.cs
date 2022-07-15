using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterManager : MonoBehaviour
	{
		[SerializeField]
		private MonsterManager _monsterManager;

		private static MonsterManager monsterManager;

		public static MonsterManager Instance => monsterManager;

		private void Awake()
		{
			monsterManager = this._monsterManager;
		}
		
		public List<MonsterController> Monsters;
	}
}