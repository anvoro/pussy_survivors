
using UnityEngine;

namespace Tank.Game.SpawnShapes
{
    internal interface ISpawnShape
    {
        Vector3 GetRandomPoint();
    }
}
