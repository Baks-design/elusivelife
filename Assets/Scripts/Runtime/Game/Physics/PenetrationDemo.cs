using System;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Extensions;
using ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers;
using UnityEngine;

namespace ElusiveLife.Game.Assets.Scripts.Runtime.Game.Physics
{
    [RequireComponent(typeof(Collider))]
    public class PenetrationDemo : MonoBehaviour
    {
        public Collider Col;
        public LayerMask ObstacleLayerMask;
        public Color MtColor = Color.yellow;
        public bool AutoResolve = true;
        public bool SmoothResolve = true;
        private Vector3 _lastCorrection;
        private bool _resolvingCollision;

        public event Action<Vector3> OnPenetrationStart;
        public event Action<Vector3> OnPenetrationStay;
        public event Action OnPenetrationEnd;

        private void Start()
        {
            OnPenetrationStart += correction =>
            {
                var penetrationDepth = correction.magnitude;
                Logging.Log(penetrationDepth);
            };
            OnPenetrationEnd += () => { Logging.Log("Penetration resolved"); };
        }

        private void Update()
        {
            var colliding = Col.GetPenetrationInLayer(ObstacleLayerMask, out var correction);
            correction += correction.normalized * 0.001f;
            _lastCorrection = colliding ? correction : Vector3.zero;

            if (colliding)
            {
                if (!_resolvingCollision)
                    OnPenetrationStart?.Invoke(correction);
                else
                    OnPenetrationStay?.Invoke(correction);

                _resolvingCollision = true;

                if (AutoResolve)
                {
                    var delta = SmoothResolve
                        ? Vector3.Lerp(
                            Vector3.zero, correction, 0.05f)
                        : correction;
                    transform.position += delta;
                }

                Logging.Log($"Colliding, MTV = {correction.magnitude:F3}");
            }
            else
            {
                if (_resolvingCollision)
                    OnPenetrationEnd?.Invoke();
                _resolvingCollision = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (Col == null)
            {
                Col = GetComponent<Collider>();
                return;
            }

            if (_lastCorrection == Vector3.zero)
                return;
            var start = Col.bounds.center;
            var end = start + _lastCorrection;
            Gizmos.color = MtColor;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.05f);
        }
    }
}