using System;
using System.Collections;
using CodeBase.Storage;
using UnityEngine;

namespace CodeBase.Factory
{
	public class FreeFactoryBase : FactoryBase
	{
		public override event Action FactoryStorageEmptied;
		public override event Action FactoryStorageFilled;

		private bool _canFactoryStoreLastTime;
		
		protected override void InitState()
		{
			bool canFactoryStore = _factoryStorage.CanStoreNew();

			if (canFactoryStore)
				FactoryStorageEmptied?.Invoke();
			else
				FactoryStorageFilled?.Invoke();
			
			_canFactoryStoreLastTime = canFactoryStore;
		}
		
		protected override void TryCreateResource()
		{
			if (CanCreate())
				CreateResource();
		}
		
		private bool CanCreate()
		{
			bool canFactoryStore = _factoryStorage.CanStoreNew();
			
			if (_canFactoryStoreLastTime == false && canFactoryStore == true)
				FactoryStorageEmptied?.Invoke();
			else if(canFactoryStore == false && _canFactoryStoreLastTime == true)
				FactoryStorageFilled?.Invoke();
			
			_canFactoryStoreLastTime = canFactoryStore;

			return canFactoryStore;
		}
	}
}