using UnityEngine;

namespace Utilities.CGTK.Greasy
{
    public static class MaterialTweens
    {
        public static Coroutine FloatTo(this Material material, string name, float to, float duration, EaseType ease)
        {
            float from = material.GetFloat(name);
            return Greasy.To(from, to, duration, ease, setter: (x => from = x));
        }

        public static Coroutine FloatTo(this Material material, string name, float to, float duration, EaseMethod ease)
        {
            float from = material.GetFloat(name);
            return Greasy.To(from, to, duration, ease, setter: (x => from = x));
        }
        
        /*
        public static MaterialPropertyBlock To(MaterialPropertyBlock from, MaterialPropertyBlock to, float duration,
        EaseType ease, Setter<Quaternion> setter)
        {
            return CreateInterpolater(
            duration,
            ease, 
            t => setter(MaterialPropertyBlock.Slerp(from, to, t)));   
        }
        
        public static MaterialPropertyBlock To(MaterialPropertyBlock from, MaterialPropertyBlock to, float duration, EaseMethod ease, Setter<Quaternion> setter)
        => CreateInterpolater(duration, ease, t => setter(MaterialPropertyBlock.SlerpUnclamped(from, to, t)));
        */
    }
}