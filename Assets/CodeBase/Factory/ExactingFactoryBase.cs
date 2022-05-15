using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Logic;
using CodeBase.Resource;
using CodeBase.Storage;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Factory
{
	public class ExactingFactoryBase : FactoryBase
	{
		public override event Action FactoryStorageFilled;
		public override event Action FactoryStorageEmptied;
		public override event Action HeroStorageEmptied;
		public override event Action HeroStorageFilled;

		[SerializeField] private HeroStorage _heroStorage;
		[SerializeField] private MoveSettings _moveSettings;
		[SerializeField] private Transform _reducePoint;
		[Space(10)] 
		[SerializeField] private List<ResourceType> _requiredResources;
		
		private bool _hasRequiredResources => _heroStorage.HasResourcePairWithTypes(_requiredResources);

		private bool _canFactoryStoreLastTime;
		private bool _hasRequiredResourcesLastTime;
		private List<ResourceType> _reducedResourceTypes = new List<ResourceType>();

		protected override void InitState()
		{
			bool hasRequiredResource = _hasRequiredResources;
			bool canFactoryStore = _factoryStorage.CanStoreNew();
			
			if(hasRequiredResource)
				HeroStorageFilled?.Invoke();
			else
				HeroStorageEmptied?.Invoke();
			
			if (canFactoryStore)
				FactoryStorageEmptied?.Invoke();
			else
				FactoryStorageFilled?.Invoke();
			
			_canFactoryStoreLastTime = canFactoryStore;
			_hasRequiredResourcesLastTime = hasRequiredResource;
		}

		protected override void TryCreateResource()
		{
			if (CanCreate())
				ReduceResources();
		}

		private void ReduceResources()
		{
			List<FactoryResource> resources = _heroStorage.GetResourcePair(_requiredResources);

			foreach (FactoryResource resource in resources)
			{
				resource.transform.DOMove(_reducePoint.position, _moveSettings.ResourceMoveDuration)
					.SetEase(_moveSettings.EaseMode)
					.OnComplete(() => OnReduceResourceComplete(resource));
			}
		}

		private void OnReduceResourceComplete(FactoryResource resource)
		{
			_reducedResourceTypes.Add(resource.Type);
			resource.transform.DOKill();
			Destroy(resource.gameObject);

			if (_reducedResourceTypes.IsContainAllElements(_requiredResources) == false) return;
			
			_reducedResourceTypes = _reducedResourceTypes.Except(_requiredResources).ToList();
			CreateResource();
		}

		private bool CanCreate()
		{
			bool hasRequiredResource = _hasRequiredResources;
			bool canFactoryStore = _factoryStorage.CanStoreNew();
			
			if(_hasRequiredResourcesLastTime == false && hasRequiredResource == true)
				HeroStorageFilled?.Invoke();
			else if(hasRequiredResource == false && _hasRequiredResourcesLastTime == true)
				HeroStorageEmptied?.Invoke();
			
			if (_canFactoryStoreLastTime == false && canFactoryStore == true)
				FactoryStorageEmptied?.Invoke();
			else if(canFactoryStore == false && _canFactoryStoreLastTime == true)
				FactoryStorageFilled?.Invoke();
			
			_canFactoryStoreLastTime = canFactoryStore;
			_hasRequiredResourcesLastTime = hasRequiredResource;

			return hasRequiredResource && canFactoryStore;
		}
	}
}