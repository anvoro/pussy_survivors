
using UnityEngine;

namespace Tank.Game.SpawnShapes
{
    public class CompositeSpawnShape : MonoBehaviour, ISpawnShape
    {
        [SerializeField]
        private SpawnRect[] _childSpawners;

        public Vector3 GetRandomPoint()
        {
            return _childSpawners[Random.Range(0, _childSpawners.Length)].GetRandomPoint();
        }
    }
}
