using System;
using System.Runtime.CompilerServices;

namespace ElusiveLife.Utils.Assets.Scripts.Runtime.Utils.Helpers
{
    public static class Mathfs
    {
        private const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;

        [MethodImpl(Inline)]
        public static float Eerp(float a, float b, float t) =>
            t switch
            {
                0f => a,
                1f => b,
                _ => MathF.Pow(a, 1f - t) * MathF.Pow(b, t)
            };
    }
}
