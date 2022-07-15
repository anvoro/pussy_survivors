using System;
using UnityEngine;

namespace DefaultNamespace
{
	public abstract class CharacterBase : MonoBehaviour
	{	
		public float Speed = 1f;

		public bool IsDead { get; set; }
		
		protected SpriteRenderer _renderer;
		
		protected Transform _transform;

		public Vector2 Velocity { get; protected set; }

		public int MaxHealth = 10;
		private int _currentHealth;

		public event Action OnHealthChanged; 
		public event Action<CharacterBase> OnCharacterDie; 

		public int CurrentHealth
		{
			get => this._currentHealth;
			private set
			{
				this._currentHealth = value;

				if (this._currentHealth < 0)
					this._currentHealth = 0;

				if (this._currentHealth > this.MaxHealth)
					this._currentHealth = this.MaxHealth;

				this.OnHealthChanged?.Invoke();
				
				if(this._currentHealth == 0)
					OnCharacterDie?.Invoke(this);
			}
		}

		public Vector2 Position => this._transform.position;

		public void Hurt(int value)
		{
			this.CurrentHealth -= Math.Abs(value);
		}
		
		public void Heal(int value)
		{
			this.CurrentHealth += Math.Abs(value);
		}
		
		protected virtual void Awake()
		{
			_renderer = this.GetComponent<SpriteRenderer>();
			this._transform = this.transform;
		}

		public virtual void Reset()
		{
			this._currentHealth = this.MaxHealth;
		}

		protected virtual void Update()
		{
			_renderer.flipX = this.Velocity.x < 0;
		}
	}
}