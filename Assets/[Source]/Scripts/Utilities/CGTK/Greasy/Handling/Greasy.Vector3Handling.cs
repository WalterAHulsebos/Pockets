using UnityEngine;

namespace Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Vector3 from, Vector3 to, float duration, EaseType ease, Setter<Vector3> setter)
            => CreateInterpolater(duration, ease, t => setter (Vector3.LerpUnclamped(from, to, t)));
        
        public static Coroutine To(Vector3 from, Vector3 to, float duration, EaseMethod ease, Setter<Vector3> setter)
            => CreateInterpolater(duration, ease, t => setter (Vector3.LerpUnclamped(from, to, t)));
		
    }
}