﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.CGTK;

namespace Utilities.Extensions
{
	public static partial class Math
	{
		#region Clamping

		public static float Clamp(this float value, float min, float max) =>
			Mathf.Clamp(value: value, min: min, max: max);

		public static float Clamp01(this float value) => Mathf.Clamp01(value: value);

		#endregion

		#region Rounding

		public static float Round(this float value, CGMath.RoundingMode mode)
			=> CGMath.Round(value: value, mode: mode);
		public static int RoundToInt(this float value, CGMath.RoundingMode mode)
			=> CGMath.RoundToInt(value: value, mode: mode);
		
		public static float Round(this float value) => Mathf.Round(value);
		public static int RoundToInt(this float value) => Mathf.RoundToInt(value);
		
		public static float Floor(this float value) => Mathf.Floor(value);
		public static int FloorToInt(this float value) => Mathf.FloorToInt(value);

		public static float Ceil(this float value) => Mathf.Ceil(value);
		public static int CeilToInt(this float value) => Mathf.CeilToInt(value);

		public static bool Approximately(this float a, float b) => Mathf.Approximately(a, b);

		public static float RoundToMultipleOf(this float value, float multiple)
			=> CGMath.RoundToMultipleOf(value: value, multiple: multiple);
		public static int RoundToMultipleIntOf(this float value, int multiple)
			=> CGMath.RoundToMultipleIntOf(value: value, multiple: multiple);

		#endregion

		public static float Abs(this float value) => Mathf.Abs(value);
		
		public static float ToAbs(ref this float value) => Mathf.Abs(value);
		
		public static float Pow(this float value, float power) => Mathf.Pow(value, power);

		public static float Snap(this float target, float snapIncrement)
			=> target = Mathf.Round(target / snapIncrement) * snapIncrement;//=> target = target - (target % snapIncrement);
		
		/*
		public static float ClosestPowerOfTen(this float value)
		{
			float absoluteValue = value.Abs;
			
			return (absoluteValue <= 0) ? 1 : Mathf.Pow(10, Mathf.Log10(absoluteValue).RoundToInt());
		}
	
		public static float ClosestPowerOfTwo(this float value)
		{
			return Mathf.ClosestPowerOfTwo(value);
		}
		*/

	}
}