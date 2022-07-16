using System.Threading.Tasks;
using Tank.Helpers.ObjectPool;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Weapons
{
	[CreateAssetMenu(fileName = "GunWeapon", menuName = "Weapons/GunWeapon", order = 0)]
	public class GunWeapon : WeaponBase
	{
		public Projectile ProjectilePrefab;

		public float ProjectileSpawnOffset = 1f;

		private ObjectPool<Projectile> _pool;
		
		public override void Init(PlayerController player)
		{
			base.Init(player);
			
			initPool();
			
			void initPool()
			{
				this._pool = new ObjectPool<Projectile>(() =>
					{
						Projectile projectile = Instantiate(this.ProjectilePrefab, Projectile.ProjectileParent);
						projectile.OnTriggered += this.OnProjectileTriggered;
						
						return projectile;
					},
					projectile =>
					{
						projectile.ResetProjectile();
					},
					(go, index) =>
					{
						go.name = string.Concat(go.name, $"_{index}");
					});

				this._pool.Create(2);
			}
		}

		private async void OnProjectileTriggered(Projectile projectile)
		{
			projectile.Collider.enabled = false;
			
			float value = 1f;
			while (value > 0)
			{
				projectile.SetDissolvePower(value);

				value -= 0.1f;
					
				await Task.Yield();
			}
			
			this._pool.Return(projectile);
		}

		protected override void Attack()
		{
			var projectile = _pool.GetOrCreate();
			projectile.Init(this.Damage);
			
			Transform transform;
			(transform = projectile.transform).rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: -this._player.Velocity);
			transform.position += (Vector3)_player.Position + (Vector3)this._player.Velocity.normalized * -this.ProjectileSpawnOffset;
			
			projectile.gameObject.SetActive(true);
		}
	}
}