using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.CGTK;

namespace Utilities.Extensions
{
	public static partial class Math
	{
		public static int Abs(this int value)
			=> Mathf.Abs(value);

		public static int ClosestPowerOfTwo(this int value)
			=> Mathf.ClosestPowerOfTwo(value);
		
	 	public static int Lerp(this int from, int to, float duration)
	 		=> CGMath.Lerp(from, to, duration);

	   public static int LerpUnclamped(this int from, int to, float duration)
		   => CGMath.LerpUnclamped(from, to, duration);

	   public static int InverseLerp(this int from, int to, float duration)
		   => CGMath.InverseLerp(from, to, duration);
	}
}
