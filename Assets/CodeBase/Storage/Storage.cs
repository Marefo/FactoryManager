using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Storage
{
	public class Storage : MonoBehaviour
	{
		[SerializeField] protected Transform _startPoint;
		[Space(10)]
		[SerializeField] private VectorDirection _rowsDirection;
		[SerializeField] private VectorDirection _columnsDirection;
		[Space(10)]
		[SerializeField] protected StorageSettings _settings;
		[SerializeField] protected MoveSettings _moveSettings;
		
		private int _currentRow => Mathf.FloorToInt(_points.Count / _settings.MaxResourcesInRow);
		private int _pointsInRow => Mathf.FloorToInt(_points.Count - Mathf.Clamp(_currentRow, 0, int.MaxValue) * _settings.MaxResourcesInRow);

		private Vector3? _lastPosition;

		private readonly List<StoragePoint> _points = new List<StoragePoint>();
		
		protected virtual void Start() => GeneratePoints();
		
		public bool CanStoreNew() => _points.Count(point => point.Available) > 0;
		
		protected StoragePoint GetAvailablePoint() => _points.FirstOrDefault(point => point.Available == true);

		private void GeneratePoints()
		{
			for (int i = 0; i < _settings.MaxRowsNumber * _settings.MaxResourcesInRow; i++) 
				CreatePoint();
		}

		private void CreatePoint()
		{
			GameObject point = new GameObject("Point");
			point.transform.SetParent(_startPoint);
			point.transform.localPosition = GetPositionForPoint();
			
			StoragePoint pointScript = point.AddComponent<StoragePoint>();
			pointScript.Available = true;
			
			_points.Add(pointScript);
		}
		
		private Vector3 GetPositionForPoint()
		{
			Vector3 position = Vector3.zero;

			if (_lastPosition != null)
			{
				if (_pointsInRow == 0)
				{
					position = Vector3.zero;
					position += _rowsDirection.GetVector() * (_currentRow * _settings.RowGap);
				}
				else
					position = (Vector3)(_lastPosition + _columnsDirection.GetVector() * _settings.ColumnGap);
			}

			_lastPosition = position;
			
			return position;
		}
	}
}