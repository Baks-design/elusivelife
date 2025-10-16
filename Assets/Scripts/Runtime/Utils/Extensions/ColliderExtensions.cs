using UnityEngine;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Extensions
{
    public static class ColliderExtensions
    {
        private static readonly Collider[] OverlapCache = new Collider[32];

        // Finds all penetrations in a layer mask and sums their MTVs
        public static bool GetPenetrationInLayer(
            this Collider source, LayerMask layerMask, out Vector3 totalCorrection)
        {
            totalCorrection = Vector3.zero;
            if (source == null)
                return false;

            var count = Physics.OverlapBoxNonAlloc(
                source.bounds.center,
                source.bounds.extents,
                OverlapCache,
                source.transform.rotation,
                layerMask);

            var collided = false;

            for (var i = 0; i < count; i++)
            {
                var other = OverlapCache[i];
                if (other == source || !source.ComputePenetration(other, out var dir, out var dist))
                    continue;

                collided = true;
                totalCorrection += dir * dist;
            }

            return collided;
        }

        //Compute the minimun translation vector (MTV) between two colliders
        public static bool ComputePenetration(
            this Collider source, Collider target, out Vector3 direction, out float distance)
        {
            direction = Vector3.zero;
            distance = 0f;
            if (source == null || target == null)
                return false;

            return Physics.ComputePenetration(
                source, source.transform.position, source.transform.rotation,
                target, target.transform.position, target.transform.rotation,
                out direction, out distance
            );
        }
    }
}
