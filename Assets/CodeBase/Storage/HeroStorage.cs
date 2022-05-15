using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Hero;
using CodeBase.Logic;
using CodeBase.Resource;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Storage
{
	public class HeroStorage : Storage
	{
		[SerializeField] private TriggerListener _zone;
		[Space(10)] 
		[SerializeField] private List<ResourceType> _takeResourceTypes;
		[SerializeField] private float _takeResourcesInterval;

		private readonly List<FactoryResource> _resources = new List<FactoryResource>();
		private HeroInventory _heroInventory;
		private Coroutine _takeResourcesCoroutine;
		private bool _inZone = false;
		
		private void OnEnable()
		{
			_zone.Entered += OnZoneEnter;
			_zone.Canceled += OnZoneCancel;
		}

		private void OnDisable()
		{
			_zone.Entered -= OnZoneEnter;
			_zone.Canceled -= OnZoneCancel;
		}

		private void OnZoneEnter(Collider obj)
		{
			if (obj.TryGetComponent(out HeroMovement heroMovement) == false || _inZone == true) return;

			_inZone = true;
			_heroInventory = heroMovement.GetComponent<HeroInventory>();
			
			if (heroMovement.IsMoving == false)
				StartTakingResources();
			else
				heroMovement.Stopped += StartTakingResources;
		}

		private void OnZoneCancel(Collider obj)
		{
			if (obj.TryGetComponent(out HeroMovement heroMovement) == false || _inZone == false) return;
			
			heroMovement.Stopped -= StartTakingResources;
			StopTakingResources();
			_inZone = false;
		}

		public bool HasResourcePairWithTypes(List<ResourceType> requiredTypes) => 
			_resources.Select(resource => resource.Type).ToList().IsContainAllElements(requiredTypes);

		public List<FactoryResource> GetResourcePair(List<ResourceType> types)
		{
			List<FactoryResource> pair = new List<FactoryResource>();

			foreach (ResourceType type in types)
			{
				FactoryResource targetTypeResource = _resources.First(resource => resource.Type == type);
				targetTypeResource.ReleasePoint();
				_resources.Remove(targetTypeResource);
				pair.Add(targetTypeResource);
			}
			
			return pair;
		}
		
		private void StartTakingResources() => 
			_takeResourcesCoroutine = StartCoroutine(TakeResourcesCoroutine());

		private void StopTakingResources()
		{
			if (_takeResourcesCoroutine != null)
				StopCoroutine(_takeResourcesCoroutine);
		}

		private IEnumerator TakeResourcesCoroutine()
		{
			while (true)
			{
				TakeResource();
				yield return new WaitForSeconds(_takeResourcesInterval);
			}
		}

		private void TakeResource()
		{
			if (_heroInventory.CanGiveResources(_takeResourceTypes) == false || CanStoreNew() == false) return;
			
			List<FactoryResource> resources = _heroInventory.GiveResources(_takeResourceTypes);
			resources.ForEach(AddResource);
		}

		private void AddResource(FactoryResource resource)
		{
			resource.CanBePicked = false;
			StoragePoint point = GetAvailablePoint();
			resource.transform.SetParent(transform);
			resource.transform.localRotation = _startPoint.localRotation;
			resource.transform.DOMove(point.transform.position, _moveSettings.ResourceMoveDuration).SetEase(_moveSettings.EaseMode);
			resource.TakePoint(point);
			
			_resources.Add(resource);
		}
	}
}