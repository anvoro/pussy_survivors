using System;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
	public class Missle : MonoBehaviour
	{
		private static Transform projectileParent;

		public float Speed;

		public float range = 10;
		
		public bool IsTriggered { get; set; }

		public bool DoDamageOnContact = true;
		public bool DestroySelfOnContact = true;
		
		private float _damage;

		private Rigidbody2D _rigidbody;

		public SpriteRenderer Sprite;
		
		public Collider2D Collider;
		public event Action<Missle> OnTriggered;
		
		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody2D>();
		}

		private MonsterController target;
		public void Init(float damage)
		{
			this._damage = damage;

			var a = MonsterManager.Instance.ActiveMonsters
				.Where(e => Vector2.Distance(e.Position, GameManager.Instance.Player.Position) <= range).ToArray();
			if(a.Length > 0)
				target = a[Random.Range(0, a.Length)];
			else
			{
				this.target = null;
			}
		}

		private void SetTriggered()
		{
			this.IsTriggered = true;
			this.OnTriggered?.Invoke(this);
		}

		private void OnTriggerEnter2D(Collider2D col)
		{
			if(this.IsTriggered == true)
				return;
			
			if (DoDamageOnContact == true && col.gameObject.TryGetComponent(out MonsterController monster))
			{
				monster.Hurt(this._damage);

				if (DestroySelfOnContact == true)
					SetTriggered();
			}
		}

		private void OnCollisionEnter2D(Collision2D col)
		{
			if(this.IsTriggered == true)
				return;
			
			if (col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
			{
				SetTriggered();
			}
		}

		private void LateUpdate()
		{
			if(this.IsTriggered == true)
				return;

			if (сheckVisibility() == false)
			{
				SetTriggered();
			}
			
			bool сheckVisibility()
			{ 
				Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
				return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
			}
		}

		private void FixedUpdate()
		{
			if(this.IsTriggered == true)
				return;
			
			if (this.target == null)
				return;
			
			var transform1 = this._rigidbody.transform;
			this._rigidbody.MovePosition(Time.deltaTime * this.Speed * (target.transform.position - transform1.position) + transform1.position);
		}

		private readonly int _dissolvePower = Shader.PropertyToID("_DissolvePower");

		public void SetDissolvePower(float value)
		{
			if(this.Sprite == null)
				return;
			
			this.Sprite.material.SetFloat(_dissolvePower, value);
		}
	}
}