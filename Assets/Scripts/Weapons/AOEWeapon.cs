using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Weapons
{
	[CreateAssetMenu(fileName = "AOEWeapon", menuName = "Weapons/AOE", order = 0)]
	public class AOEWeapon : WeaponBase
	{
		public GameObject AOEPrefab;
		
		public float range;

		private GameObject vfx;
		public override void Init(PlayerController player)
		{
			if(this.vfx == null)
				vfx =Instantiate(AOEPrefab, player.transform);
			
			vfx.transform.localScale = new Vector3(range, range, range);
			
			base.Init(player);
		}

		protected override void Attack()
		{
			foreach (MonsterController monster in MonsterManager.Instance.ActiveMonsters.Where(e => Vector2.Distance(e.Position, this._player.Position) <= range))
			{
				monster.Hurt(this.Damage);
			}
		}

		public override void DrawG()
		{
			if (this._player == null)
				return;
			
			Gizmos.DrawWireSphere(this._player.transform.position, range);
		}
	}
}