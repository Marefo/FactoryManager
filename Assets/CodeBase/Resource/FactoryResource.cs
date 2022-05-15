using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Resource
{
	public class FactoryResource : MonoBehaviour
	{
		[HideInInspector] public bool CanBePicked = true;
		
		[field: SerializeField] public ResourceType Type { get; private set; }
		
		private StoragePoint _point;
		
		public void TakePoint(StoragePoint point)
		{
			_point = point;
			_point.Available = false;
		}

		public void ReleasePoint() => _point.Available = true;
	}
}