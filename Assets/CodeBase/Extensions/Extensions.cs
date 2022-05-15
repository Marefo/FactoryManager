using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Extensions
{
	public static class Extensions
	{
		public static Vector3 GetVector(this VectorDirection direction)
		{
			return direction switch
			{
				VectorDirection.Right => Vector3.right,
				VectorDirection.Left => Vector3.left,
				VectorDirection.Up => Vector3.up,
				VectorDirection.Down => Vector3.down,
				VectorDirection.Forward => Vector3.forward,
				VectorDirection.Back => Vector3.back,
				_ => Vector3.zero
			};
		}

		public static bool IsContainAllElements<T>(this List<T> list, List<T> requiredElements)
		{
			bool hasAllRequiredElements = true;

			foreach (T element in requiredElements)
			{
				bool hasCurrentElement = list.Count(currentElement => EqualityComparer<T>.Default.Equals(currentElement, element)) > 0;
				
				if(hasCurrentElement == true) continue;

				hasAllRequiredElements = false;
				break;
			}

			return hasAllRequiredElements;
		}
	}
}