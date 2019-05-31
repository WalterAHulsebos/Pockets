using UnityEngine;

namespace Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Vector4 from, Vector4 to, float duration, EaseType ease, Setter<Vector4> setter)
        {
            return CreateInterpolater (duration, ease, t => setter (Vector4.LerpUnclamped (from, to, t)));
        }
        public static Coroutine To(Vector4 from, Vector4 to, float duration, EaseMethod ease, Setter<Vector4> setter)
        {
            return CreateInterpolater (duration, ease, t => setter (Vector4.LerpUnclamped (from, to, t)));
        }
		
    }
}