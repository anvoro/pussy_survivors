using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterManager : MonoBehaviour
	{
		[SerializeField]
		private MonsterManager _monsterManager;

		private static MonsterManager gm;

		public static MonsterManager Instance => gm;

		private void Awake()
		{
			gm = this._monsterManager;
		}
		
		public List<MonsterController> Monsters;
	}
}