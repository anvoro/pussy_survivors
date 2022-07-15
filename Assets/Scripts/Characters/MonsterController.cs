using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterController : CharacterBase
	{
		[SerializeField] private float _desiredSeparation = 2f;

		public int Damage = 5;

		public float AttackCD = 1f;

		private float _currentCD = 0;
		
		public MonsterController Prefab { get; set; }

		private void OnTriggerStay2D(Collider2D other)
		{
			if (_currentCD <= 0 && other.gameObject.TryGetComponent(out PlayerController player))
			{
				player.Hurt(Damage);

				this._currentCD = this.AttackCD;
			}
		}
		
		protected override void Update()
		{
			if (this._currentCD > 0)
				this._currentCD -= Time.deltaTime;
			
			Vector2 desiredVelocity = GameManager.Instance.Player.Position - this.Position + CalculateSeparationVelocity();
			this.Velocity = -desiredVelocity;
			
			this._transform.position = (Time.deltaTime * this.Speed * desiredVelocity.normalized) + this.Position;

			base.Update();
			
			Vector2 CalculateSeparationVelocity()
			{
				if(MonsterManager.Instance.ActiveMonsters.Count < 1)
					return Vector2.zero;
				
				Vector3 totalSeparation = Vector3.zero;
				int numNeighbors = 0;

				foreach (var monster in MonsterManager.Instance.ActiveMonsters)
				{
					float distance = Vector3.Distance(this.Position, monster.transform.position);
				
					if (distance < this._desiredSeparation)
					{
						Vector3 separationVector = this._transform.position - monster.transform.position;

						totalSeparation += separationVector;

						numNeighbors++;
					}
				}
			
				if (numNeighbors > 0)
				{
					return totalSeparation / numNeighbors;
				}
				
				return Vector2.zero;
			}
		}
	}
}