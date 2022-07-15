
using System.Collections.Generic;
using UnityEngine;
using Tank.Helpers;

namespace Tank.Game.SpawnShapes
{
    internal class SpawnRect : MonoBehaviour, ISpawnShape
    {
        private readonly List<Vector3> _resultVariants = new List<Vector3>(4);

        private BoxCollider2D _boxCollider;

        public Vector3 Center => transform.TransformPoint(this._boxCollider.offset);

        private void Awake()
        {
            this._boxCollider = this.GetComponent<BoxCollider2D>();
        }

        public Vector3 GetRandomPoint()
        {
            this._resultVariants.Clear();
            for (int i = 0; i < 4; i++)
            {
                this._resultVariants.Add(this._boxCollider.GetRandomPointInsideCollider());
            }
            
            return _resultVariants[Random.Range(0, _resultVariants.Count)];
        }
    }
}
