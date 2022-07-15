using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Weapons
{
	[CreateAssetMenu(fileName = "GunWeapon", menuName = "Weapons/GunWeapon", order = 0)]
	public class GunWeapon : WeaponBase
	{
		public Projectile ProjectilePrefab;

		public float ProjectileSpawnOffset = 1f;
		
		protected override void Attack()
		{
			Projectile instance = Instantiate(ProjectilePrefab);
			instance.Init(this.Damage);
			
			Transform transform;
			(transform = instance.transform).rotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: -this._player.Velocity);
			transform.position += (Vector3)_player.Position + (Vector3)this._player.Velocity.normalized * -this.ProjectileSpawnOffset;
		}
	}
}