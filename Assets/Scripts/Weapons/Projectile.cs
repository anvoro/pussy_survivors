using DefaultNamespace;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Weapons
{
	public class Projectile : MonoBehaviour
	{
		public float Speed;
		
		private float _damage;

		public float SpriteRotateSpeed = -1;

		private Rigidbody2D _rigidbody;

		public SpriteRenderer Sprite;
		
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
		
		private void OnDisable()
		{
			_tween.Kill();
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