using UnityEngine;

namespace DefaultNamespace
{
	public class MonsterController : MonoBehaviour
	{
		public float Speed = 1f;
		[SerializeField] private float _desiredSeparation = 2f;
		
		private Transform _transform;

		public Vector2 Position => this._transform.position;
    
		// Start is called before the first frame update
		void Awake()
		{
			this._transform = this.transform;
		}
		
		private void Update()
		{
			Vector2 desiredVelocity = /*(Vector2)GameManager.Instance.Player.Position - this.Position +*/ CalculateSeparationVelocity();
			
			this._transform.position = (Time.deltaTime * this.Speed * desiredVelocity.normalized) + this.Position;

			Vector2 CalculateSeparationVelocity()
			{
				Vector3 totalSeparation = Vector3.zero;
				int numNeighbors = 0;

				foreach (var monster in MonsterManager.Instance.Monsters)
				{
					float distance = Vector3.Distance(this.Position, monster.transform.position);
				
					if (distance < this._desiredSeparation)
					{
						Vector3 separationVector = this._transform.position - monster.transform.position;
						//todo: не ясно влияет ли это деление на что то, надо провести больше тестов
						//separationVector /= distance;

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