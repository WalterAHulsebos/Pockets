using UnityEngine;
using Utilities.CGTK;
using Utilities.Extensions;
using static Utilities.Extensions.Math;

namespace Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Vector3Int from, Vector3Int to, float duration, EaseType ease, Setter<Vector3Int> setter)
        {
            return CreateInterpolater(duration, ease, t => setter(CGMath.Lerp(from, to, t)));
        }
        public static Coroutine To(Vector3Int from, Vector3Int to, float duration, EaseMethod ease, Setter<Vector3Int> setter)
        {
            return CreateInterpolater(duration, ease, t => setter(CGMath.LerpUnclamped(from, to, t)));
        }
    }
}