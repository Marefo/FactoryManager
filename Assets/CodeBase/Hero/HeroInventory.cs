using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Logic;
using CodeBase.Resource;
using CodeBase.Storage;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroInventory : MonoBehaviour
	{
		[SerializeField] private TriggerListener _pickUpZone;
		[Space(10)]
		[SerializeField] private int _maxResourcesNumber;
		[SerializeField] private int _maxResourcesInStack;
		[SerializeField] private Transform _stackStartPoint;
		[SerializeField] private float _verticalGap;
		[SerializeField] private float _horizontalGap;
		[Space(10)]
		[SerializeField] private float _moveDuration;
		[SerializeField] private Ease _easeMode;

		private int _currentStack => Mathf.FloorToInt(_resources.Count / _maxResourcesInStack);
		private int _resourcesInStack => Mathf.FloorToInt(_resources.Count - Mathf.Clamp(_currentStack, 0, int.MaxValue) * _maxResourcesInStack);

		private List<FactoryResource> _resources = new List<FactoryResource>();
		private Vector3 _lastResourcePosition;
		
		private void OnEnable() => _pickUpZone.Entered += OnPickUpZoneEnter;

		private void OnDisable() => _pickUpZone.Entered -= OnPickUpZoneEnter;

		public bool CanGiveResources(List<ResourceType> requiredTypes) => 
			_resources.Select(resource => resource.Type).ToList().IsContainAllElements(requiredTypes);

		public List<FactoryResource> GiveResources(List<ResourceType> types)
		{
			List<FactoryResource> pair = new List<FactoryResource>();

			foreach (ResourceType type in types)
			{
				FactoryResource targetTypeResource = _resources.Last(resource => resource.Type == type);
				RemoveResource(targetTypeResource);
				pair.Add(targetTypeResource);
			}

			return pair;
		}

		private void RemoveResource(FactoryResource resource)
		{
			Vector3 resourcePosition = resource.transform.localPosition;
			bool isLast = _resources.IndexOf(resource) == _resources.Count - 1;

			if (isLast == false)
			{
				FactoryResource _lastResourceInStack = _resources.Last();
				_lastResourceInStack.transform.localPosition = resourcePosition;

				_resources[_resources.IndexOf(resource)] = _lastResourceInStack;
			}
			
			_resources.RemoveAt(_resources.Count - 1);

			_lastResourcePosition = _resources.Count > 0 ? _resources.Last().transform.localPosition : Vector3.zero;
		}

		private void OnPickUpZoneEnter(Collider obj)
		{
			if(obj.TryGetComponent(out FactoryResource resource) == false) return;
			if(_resources.Count >= _maxResourcesNumber || resource.CanBePicked == false) return;
			
			PickUp(resource);
		}

		private void PickUp(FactoryResource factoryResource)
		{
			Vector3 targetPosition = Vector3.zero;

			if (_resources.Count != 0)
			{
				targetPosition = _lastResourcePosition + Vector3.up * _verticalGap;
				
				if (_resourcesInStack == 0)
				{
					targetPosition = Vector3.zero;
					targetPosition.z -= _horizontalGap;
				}
			}

			_lastResourcePosition = targetPosition;
			_resources.Add(factoryResource);
			
			factoryResource.ReleasePoint();
			factoryResource.transform.DOKill();
			factoryResource.transform.SetParent(_stackStartPoint);
			factoryResource.transform.localRotation = Quaternion.Euler(Vector3.zero);
			factoryResource.transform.DOLocalMove(targetPosition, _moveDuration).SetEase(_easeMode);
		}
	}
}