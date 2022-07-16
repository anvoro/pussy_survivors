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
			
			projectile.OnTriggered -= this.OnProjectileTriggered;
			Destroy(projectile.gameObject);
		}

		protected override void Attack()
		{
			var projectile = Instantiate(this.ProjectilePrefab, Projectile.ProjectileParent);
			projectile.Init(this.Damage);
			
			projectile.OnTriggered += this.OnProjectileTriggered;
			Transform transform = projectile.transform;
			transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: -this._player.Velocity);
			transform.position += (Vector3)_player.Position + (Vector3)this._player.Velocity.normalized * -this.ProjectileSpawnOffset;
		}

		public override void DrawG()
		{
		}
	}
}