using CodeBase.Logic;
using CodeBase.Resource;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Storage
{
	public class FactoryStorage : Storage
	{
		public void AddResource(GameObject resource)
		{
			FactoryResource resourceScript = resource.GetComponent<FactoryResource>();
			
			StoragePoint point = GetAvailablePoint();
			resource.transform.rotation = _startPoint.rotation;
			resource.transform.DOMove(point.transform.position, _moveSettings.ResourceMoveDuration).SetEase(_moveSettings.EaseMode);
			resourceScript.TakePoint(point);
		}
	}
}