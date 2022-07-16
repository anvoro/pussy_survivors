using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class GameManager : MonoBehaviour
	{
		public PlayerController Player;

		public MonsterManager MonsterManager;
		
		[SerializeField]
		private GameManager _gameManager;

		private static GameManager gm;

		public static GameManager Instance => gm;

		private void Awake()
		{
			gm = _gameManager;
			MonsterManager.Init();
		}
	}
}