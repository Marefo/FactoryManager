using System;
using System.Collections;
using CodeBase.Storage;
using UnityEngine;

namespace CodeBase.Factory
{
	public abstract class FactoryBase : MonoBehaviour
	{
		public virtual event Action FactoryStorageFilled;
		public virtual event Action FactoryStorageEmptied;
		public virtual event Action HeroStorageEmptied;
		public virtual event Action HeroStorageFilled;
		
		[SerializeField] protected GameObject _resourcePrefab;
		[SerializeField] protected Transform _creationPoint;
		[Space(10)]
		[SerializeField] protected FactoryStorage _factoryStorage;
		[SerializeField] protected float _creationInterval;
		
		private Coroutine _creationCoroutine;
		
		protected virtual void Start()
		{
			InitState();
			_creationCoroutine = StartCoroutine(CreateCoroutine());
		}

		protected virtual void OnDestroy() => StopCoroutine(_creationCoroutine);

		protected abstract void InitState();
		protected abstract void TryCreateResource();


		private IEnumerator CreateCoroutine()
		{
			while (true)
			{	
				yield return new WaitForSeconds(_creationInterval);
				TryCreateResource();
			}
		}

		protected void CreateResource()
		{
			GameObject resource = Instantiate(_resourcePrefab, _creationPoint.position, Quaternion.identity);
			_factoryStorage.AddResource(resource);
		}
	}
}