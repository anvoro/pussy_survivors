using UnityEngine;

namespace Weapons
{
	public abstract class WeaponBase : ScriptableObject
	{
		public float DamageMult = 1;

		public float CD = 1f;
		
		private float _currentCD = 0;

		protected PlayerController _player;
		
		public virtual void Init(PlayerController player)
		{
			_player = player;
		}
		
		public void Tick(float deltaTime)
		{
			if (this._currentCD > 0)
				this._currentCD -= deltaTime;
			
			if (_currentCD <= 0)
			{
				this.Attack();
				this._currentCD = this.CD;
			}
		}

		protected abstract void Attack();

		public abstract void DrawG();
	}
}