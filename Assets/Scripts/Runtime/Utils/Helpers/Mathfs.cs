using System.Runtime.CompilerServices;
using UnityEngine;

namespace ElusiveLife.Runtime.Utils.Helpers
{
    public static class Mathfs
    {
        private const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

        /// <summary>Exponential interpolation, the multiplicative version of lerp, useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="t">The t-value from 0 to 1 representing position along the eerp</param>
        [MethodImpl(Inline)]
        public static float Eerp(float a, float b, float t) =>
            t switch
            {
                0f => a,
                1f => b,
                _ => Mathf.Pow(a, 1f - t) * Mathf.Pow(b, t)
            };

        /// <summary>Inverse exponential interpolation, the multiplicative version of InverseLerp, useful for values such as scaling or zooming</summary>
        /// <param name="a">The start value</param>
        /// <param name="b">The end value</param>
        /// <param name="v">A value between a and b. Note: values outside this range are still valid, and will be extrapolated</param>
        [MethodImpl(Inline)]
        public static float InverseEerp(float a, float b, float v)
            => Mathf.Log(a / v) / Mathf.Log(a / b);

        public static float ExpDecay(float a, float b, float dt, float decay = 16f)
            => b + (a - b) * Mathf.Exp(-decay * dt);

        public static Vector3 ExpDecay(Vector3 a, Vector3 b, float dt, float decay = 16f)
            => b + (a - b) * Mathf.Exp(-decay * dt);

        public static Vector2 ExpDecay(Vector2 a, Vector2 b, float dt, float decay = 16f)
            => b + (a - b) * Mathf.Exp(-decay * dt);

        public static float LerpAngle(float current, float target, float deltaTime, float sharpness = 3f)
        {
            var difference = Mathf.DeltaAngle(current, target);
            var factor = 1f - Mathf.Exp(-sharpness * deltaTime);
            return current + difference * factor;
        }
    }
}
