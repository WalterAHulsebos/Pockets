using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.CGTK;

namespace Utilities.Extensions
{
	public static partial class Math
	{
		//TODO: Vector4
		
		#region Vector3
			
		//public static Vector3 MaxValue(this Vector3 vector) => new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);	
		
		public static Vector3 Closest(this IEnumerable<Vector3> vectors, Vector3 comparer)
		{
			Vector3 closestVector = Vector3.positiveInfinity;
			float closestDistance = float.MaxValue;

			foreach (Vector3 vector in vectors)
			{
				float distance = vector.DistanceTo(comparer);

				if (!(distance <= closestDistance)) continue;
				
				closestDistance = distance;
				closestVector = vector;
			}
			
			return closestVector;
		}
		
		//public static Vector3 GetClosest(this List<Vector3> vectors, Vector3 comparer) => vectors.OrderBy(vector => Math.Abs(vector - comparer)).First();

		public static float DistanceTo(this Vector3 from, Vector3 to)
			=> Vector3.Distance(from, to);
		
		#region Rounding
		
		public static void Round(this Vector3 v) => v.Rounded();
		
		public static void Round(this Vector3 v, CGMath.RoundingMode roundingMode) => v.Rounded(roundingMode);

		public static void Floor(this Vector3 v) => v.Floored();

		public static void Ceil(this Vector3 v) => v.Ceiled();
		
		public static Vector3 Rounded(this Vector3 v) 
			=> new Vector3 {x = v.x.Round(), y = v.y.Round(), z = v.z.Round()};
		
		public static Vector3 Rounded(this Vector3 v, CGMath.RoundingMode roundingMode) 
			=> new Vector3 {x = v.x.Round(roundingMode), y = v.y.Round(roundingMode), z = v.z.Round(roundingMode)};
		
		public static Vector3Int RoundedToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.RoundToInt(), y = v.y.RoundToInt(), z = v.z.RoundToInt()};
		
		public static Vector3Int RoundedToVector3Int(this Vector3 v, CGMath.RoundingMode roundingMode)
			=> new Vector3Int {x = v.x.RoundToInt(), y = v.y.RoundToInt(), z = v.z.RoundToInt()};

		public static Vector3 Floored(this Vector3 v)
			=> new Vector3 {x = v.x.Floor(), y = v.y.Floor(), z = v.z.Floor()};
		
		public static Vector3Int FlooredToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.FloorToInt(), y = v.y.FloorToInt(), z = v.z.FloorToInt()};
		
		public static Vector3 Ceiled(this Vector3 v)
			=> new Vector3 {x = v.x.Ceil(), y = v.y.Ceil(), z = v.z.Ceil()};
		
		public static Vector3Int CeiledToVector3Int(this Vector3 v)
			=> new Vector3Int {x = v.x.CeilToInt(), y = v.y.CeilToInt(), z = v.z.CeilToInt()};
		
		#endregion
		
		public static Vector3 GetRelativePositionFrom(this Vector3 position, Matrix4x4 from)
			=> from.MultiplyPoint(position);

		public static Vector3 GetRelativePositionTo(this Vector3 position, Matrix4x4 to)
			=> to.inverse.MultiplyPoint(position);

		public static Vector3 GetRelativeDirectionFrom(this Vector3 direction, Matrix4x4 from)
			=> from.MultiplyVector(direction);

		public static Vector3 GetRelativeDirectionTo(this Vector3 direction, Matrix4x4 to)
			=> to.inverse.MultiplyVector(direction);
		
		/// <summary>
		/// Mirrors a Vector in desired  Axis
		/// </summary>
		/// <returns></returns>
		public static Vector3 Mirror(this Vector3 vector, Vector3 axis) //TODO: Edit to *actually* use an axis.
		{
			if (axis == Vector3.right) { vector.x *= -1f; }

			if (axis == Vector3.up) { vector.y *= -1f; }

			if (axis == Vector3.forward) { vector.z *= -1f; }

			return vector;
		}

		public static Vector3 ToAbs(ref this Vector3 vector)
		{
			vector.x.ToAbs();
			vector.y.ToAbs();
			vector.z.ToAbs();
			
			return vector;
		}
		
		public static Vector3 Abs(this Vector3 vector) 
			=> new Vector3(vector.x.ToAbs(), vector.y.ToAbs(), vector.z.ToAbs());

		public static bool Approximately(this Vector3 position, Vector3 comparer)
			=> position.x.Approximately(comparer.x) && position.y.Approximately(comparer.y) && position.z.Approximately(comparer.z);
		
		#endregion

		#region Vector3Int

		public static Vector3Int Lerp(this Vector3Int target, Vector3Int from, Vector3Int to, float duration)
			=> CGMath.Lerp(from, to, duration);

		public static Vector3Int LerpUnclamped(this Vector3Int target, Vector3Int from, Vector3Int to, float duration)
			=> CGMath.LerpUnclamped(from, to, duration);

		public static Vector3Int InverseLerp(this Vector3Int target, Vector3Int from, Vector3Int to, float duration)
			=> CGMath.InverseLerp(from, to, duration);

		#endregion
		
		#region Vector2
		
		#region Rounding
		
		public static void Round(this Vector2 v) => v.Rounded();
		
		public static void Round(this Vector2 v, CGMath.RoundingMode roundingMode) => v.Rounded(roundingMode);

		public static void Floor(this Vector2 v) => v.Floored();

		public static void Ceil(this Vector2 v) => v.Ceiled();
		
		public static Vector2 Rounded(this Vector2 v) 
			=> new Vector2 {x = v.x.Round(), y = v.y.Round()};
		
		public static Vector2 Rounded(this Vector2 v, CGMath.RoundingMode roundingMode) 
			=> new Vector3 {x = v.x.Round(roundingMode), y = v.y.Round(roundingMode)};
		
		public static Vector2Int RoundedToVector3Int(this Vector2 v) 
			=> new Vector2Int {x = v.x.RoundToInt(), y = v.y.RoundToInt()};
		
		public static Vector2Int RoundedToVector3Int(this Vector2 v, CGMath.RoundingMode roundingMode) 
			=> new Vector2Int {x = v.x.RoundToInt(), y = v.y.RoundToInt()};

		public static Vector2 Floored(this Vector2 v) 
			=> new Vector2 {x = v.x.Floor(), y = v.y.Floor()};
		
		public static Vector2Int FlooredToVector2Int(this Vector2 v) 
			=> new Vector2Int {x = v.x.FloorToInt(), y = v.y.FloorToInt()};
		
		public static Vector2 Ceiled(this Vector2 v) 
			=> new Vector2 {x = v.x.Ceil(), y = v.y.Ceil()};
	
		public static Vector2Int CeiledToVector2Int(this Vector2 v) 
			=> new Vector2Int {x = v.x.CeilToInt(), y = v.y.CeilToInt()};
		
		#endregion
		
		public static Vector2 GetRelativePositionFrom(this Vector2 position, Matrix4x4 from) 
			=> from.MultiplyPoint(position);

		public static Vector2 GetRelativePositionTo(this Vector2 position, Matrix4x4 to) 
			=> to.inverse.MultiplyPoint(position);

		public static Vector2 GetRelativeDirectionFrom(this Vector2 direction, Matrix4x4 from) 
			=> from.MultiplyVector(direction);

		public static Vector2 GetRelativeDirectionTo(this Vector2 direction, Matrix4x4 to) 
			=> to.inverse.MultiplyVector(direction);
		
		#endregion
	}
}