using System;
using DefaultNamespace;
using UnityEngine;

namespace Weapons
{
	public class Projectile : MonoBehaviour
	{
		public float Speed;
		
		private int _damage;

		private Rigidbody2D _rigidbody;
		
		private void Awake()
		{
			_rigidbody = this.GetComponent<Rigidbody2D>();
		}

		public void Init(int damage)
		{
			this._damage = damage;
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.TryGetComponent(out MonsterController monster))
			{
				monster.Hurt(this._damage);
				
				Destroy(this.gameObject);
			}
		}

		private void FixedUpdate()
		{
			_rigidbody.MovePosition(Time.deltaTime * this.Speed * _rigidbody.transform.up + _rigidbody.transform.position);
		}
	}
}