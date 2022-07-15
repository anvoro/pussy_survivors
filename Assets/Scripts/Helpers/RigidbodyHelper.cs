
using UnityEngine;

namespace Tank.Helpers
{
    public static class RigidbodyHelper
    {
        public static void ResetRigidbody(this Rigidbody rigidbody)
        {
            rigidbody.ResetCenterOfMass();

            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public static void SetTransform(this Rigidbody rigidbody, Vector3 position, Quaternion rotation)
        {
            rigidbody.gameObject.transform.SetPositionAndRotation(position, rotation);

            rigidbody.position = position;
            rigidbody.rotation = rotation;
        }
    }
}
