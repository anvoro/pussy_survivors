
using UnityEngine;

namespace Tank.Helpers
{
    public static class BoxColliderHelper
    {
        public static Vector3 GetRandomPointInsideCollider(this BoxCollider2D boxCollider)
        {
            Vector2 extents = boxCollider.size / 2f;
            Vector2 point = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y));

            point += boxCollider.offset;

            return boxCollider.transform.TransformPoint(point);
        }

        public static Rect GetRectInXZAxis(this BoxCollider2D boxCollider)
        {
            Vector3 extents = boxCollider.size / 2f;

            float minX = boxCollider.offset.x - extents.x;
            float minZ = boxCollider.offset.y - extents.z;

            Vector3 minCorner = new Vector3(minX, 0f, minZ);

            Vector3 worldMinCorner = boxCollider.transform.TransformPoint(minCorner);

            return new Rect(worldMinCorner.x, worldMinCorner.z, boxCollider.size.x, boxCollider.size.y);
        }
    }
}
