using System;
using DefaultNamespace;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Weapons
{
	public class Projectile : MonoBehaviour
	{
		private static Transform projectileParent;
		
		public static Transform ProjectileParent => projectileParent ??= GameObject.FindGameObjectWithTag("ProjectileParent").transform;

		public float Speed;

		public bool IsTriggered { get; set; }

		public bool DoDamageOnContact = true;
		public bool DestroySelfOnContact = true;
		
		private float _damage;

		public float SpriteRotateSpeed = -1;

		private Rigidbody2D _rigidbody;

		public SpriteRenderer Sprite;
		
		public Collider2D Collider;
		public event Action<Projectile> OnTriggered;
		
		private void Awake()
		{
			this._rigidbody = this.GetComponent<Rigidbody2D>();
		}

		private TweenerCore<Quaternion, Vector3, QuaternionOptions> _tween;
		public void Init(float damage)
		{
			this._damage = damage;

			if (SpriteRotateSpeed > 0)
				_tween = this.Sprite.transform
					.DOLocalRotate(new Vector3(0, 0, 360), SpriteRotateSpeed, RotateMode.FastBeyond360)
					.SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);
		}

		private void SetTriggered()
		{
			_tween?.Kill();
			
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
			
			var transform1 = this._rigidbody.transform;
			this._rigidbody.MovePosition(Time.deltaTime * this.Speed * transform1.up + transform1.position);
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