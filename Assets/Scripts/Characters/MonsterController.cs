using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterController : CharacterBase
	{
		[SerializeField] private float _desiredSeparation = 2f;

		public float Damage = 5;

		public float AttackCD = 1f;

		private float _currentCD = 0;

		public int KillXP = 1;
		
		[SerializeField]
		private Collider2D _collider;

		public Collider2D Collider2D => this._collider;
		
		public MonsterController Prefab { get; set; }

		private readonly int _dissolvePower = Shader.PropertyToID("_DissolvePower");

		public void SetDissolvePower(float value)
		{
			if(this._renderer == null)
				return;
			
			this._renderer.material.SetFloat(_dissolvePower, value);
		}

		public override void Reset()
		{
			this._renderer.material.SetFloat(_dissolvePower, 1);
			this._collider.enabled = true;
			
			base.Reset();
		}

		private void OnCollisionStay(Collision collisionInfo)
		{
			if (_currentCD <= 0 && collisionInfo.gameObject.TryGetComponent(out PlayerController player))
			{
				player.Hurt(Damage);

				this._currentCD = this.AttackCD;
			}
		}

		// private void OnCollisionStay2D(Collider2D col)
		// {
		//
		// }
		//
		protected override void Update()
		{
			if(this.IsDead == true)
				return;
			
			if (this._currentCD > 0)
				this._currentCD -= Time.deltaTime;
			
			Vector3 desiredVelocity = GameManager.Instance.Player.Position - this.Position + CalculateSeparationVelocity();
			this.Velocity = -desiredVelocity;
			
			this.transform.position += (Time.deltaTime * this.Speed * desiredVelocity.normalized);

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
						Vector3 separationVector = transform.position - monster.transform.position;

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