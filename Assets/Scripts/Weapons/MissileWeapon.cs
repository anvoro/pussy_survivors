using System.Threading.Tasks;
using UnityEngine;

namespace Weapons
{
	[CreateAssetMenu(fileName = "MissileWeapon", menuName = "Weapons/MissileWeapon", order = 0)]
	public class MissileWeapon : WeaponBase
	{
		public Missle ProjectilePrefab;

		public float ProjectileSpawnOffset = 1f;

		public int attackCount;

		private async void OnProjectileTriggered(Missle projectile)
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
			for (int i = 0; i < attackCount; i++)
			{
				attack();
			}
			
			void attack()
			{
				var projectile = Instantiate(this.ProjectilePrefab, Projectile.ProjectileParent);
				projectile.Init(this._player.Damage * this.DamageMult);

				projectile.OnTriggered += this.OnProjectileTriggered;
				Transform transform = projectile.transform;
				transform.rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: -this._player.Velocity);
				transform.position += (Vector3)this._player.Position +
				                      (Vector3)this._player.Velocity.normalized * -this.ProjectileSpawnOffset;
			}
		}

		public override void DrawG()
		{
		}
	}
}