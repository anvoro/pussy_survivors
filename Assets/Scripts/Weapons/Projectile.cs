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

		private static Camera Camera => camera ??= Camera.main;
		private static Camera camera;
		
		public float Speed;

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
			_rigidbody = this.GetComponent<Rigidbody2D>();
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
		
		public void ResetProjectile()
		{
			_tween.Kill();

			this.gameObject.SetActive(false);
			this.Collider.enabled = true;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (DoDamageOnContact == true && other.gameObject.TryGetComponent(out MonsterController monster))
			{
				monster.Hurt(this._damage);
				
				if(DestroySelfOnContact == true)
					this.OnTriggered?.Invoke(this);
			}
		}

		private void FixedUpdate()
		{
			if (сheckInvisibility() == true)
			{
				this.OnTriggered?.Invoke(this);
				return;
			}
			
			var transform1 = _rigidbody.transform;
			_rigidbody.MovePosition(Time.deltaTime * this.Speed * transform1.up + transform1.position);
			
			bool сheckInvisibility()
			{ 
				Vector3 screenPos = Camera.WorldToScreenPoint(transform.position);
				return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
			}
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