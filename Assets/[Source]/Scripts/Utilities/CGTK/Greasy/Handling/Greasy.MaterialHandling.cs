﻿using UnityEngine;

namespace Utilities.CGTK.Greasy
{
    public static partial class Greasy
    {
        public static Coroutine To(Material material, string name, float from, float to, float duration, EaseType ease
            , Setter<float> setter)
                => CreateInterpolater(duration, ease, t => setter(Mathf.LerpUnclamped(from, to, t)));
        
        /*
        public static Coroutine To(float from, float to, float duration, EaseType ease, Setter<float> setter)
            => CreateInterpolater(duration, ease, t => setter(Mathf.LerpUnclamped(from, to, t)));
        
        public static Coroutine To(float from, float to, float duration, EaseMethod ease, Setter<float> setter)
            => CreateInterpolater(duration, ease, t => setter(Mathf.LerpUnclamped(from, to, t)));
        */
        
    }
}
