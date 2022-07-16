using System;
using UnityEngine;

namespace DefaultNamespace
{
	public abstract class CharacterBase : MonoBehaviour
	{
		public bool IsDead { get; set; }

		protected SpriteRenderer _renderer;

		private Transform Transform => this._transform ? this._transform : (this._transform = this.transform);

		public Vector2 Velocity { get; protected set; }

		public float _speedBase = 1f;
		public float _maxHealthBase = 10;

		protected float _currentHealth;
		private Transform _transform;

		public event Action OnHealthChanged;
		public event Action<CharacterBase> OnCharacterDie;

		public virtual float Speed
		{
			get => this._speedBase;
		}

		public virtual float MaxHealth
		{
			get => this._maxHealthBase;
		}

		public float CurrentHealth
		{
			get => this._currentHealth;
			private set
			{
				this._currentHealth = value;

				if (this._currentHealth < 0)
					this._currentHealth = 0;

				if (this._currentHealth > this._maxHealthBase)
					this._currentHealth = this._maxHealthBase;

				this.OnHealthChanged?.Invoke();

				if (this._currentHealth == 0)
					OnCharacterDie?.Invoke(this);
			}
		}

		public Vector2 Position => this.Transform.position;

		public void Hurt(float value)
		{
			this.CurrentHealth -= Math.Abs(value);
		}

		public void Heal(float value)
		{
			this.CurrentHealth += Math.Abs(value);
		}

		protected virtual void Awake()
		{
			_renderer = this.GetComponent<SpriteRenderer>();
		}

		public virtual void Reset()
		{
			this._currentHealth = this._maxHealthBase;
		}

		protected virtual void Update()
		{
			_renderer.flipX = this.Velocity.x < 0;
		}
	}
}