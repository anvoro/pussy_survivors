using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			Player.OnCharacterDie += c =>
			{
				SceneManager.LoadScene(0);
			};
			
			gm = _gameManager;
			MonsterManager.Init();
		}
	}
}