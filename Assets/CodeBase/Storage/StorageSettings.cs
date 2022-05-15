using DG.Tweening;
using UnityEngine;

namespace CodeBase.Storage
{
	[CreateAssetMenu(fileName = "StorageSettings", menuName = "Settings/StorageSettings")]
	public class StorageSettings : ScriptableObject
	{
		public float MaxRowsNumber;
		public float MaxResourcesInRow;
		public float ColumnGap;
		public float RowGap;
	}
}