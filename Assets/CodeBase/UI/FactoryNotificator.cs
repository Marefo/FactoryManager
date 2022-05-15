using CodeBase.Factory;
using CodeBase.Logic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
	public class FactoryNotificator : MonoBehaviour
	{
		[SerializeField] private FactoryBase _factory;
		[SerializeField] private TextMeshProUGUI _heroStorageNotification;
		[SerializeField] private TextMeshProUGUI _factoryStorageNotification;
		[Space(10)]
		[SerializeField] private PunchScaleEffectSettings _settings;

		private void OnEnable()
		{
			_factory.FactoryStorageFilled += ShowFactoryStorageNotification;
			_factory.FactoryStorageEmptied += HideFactoryStorageNotification;
			_factory.HeroStorageEmptied += ShowHeroStorageNotification;
			_factory.HeroStorageFilled += HideHeroStorageNotification;
		}

		private void OnDisable()
		{
			_factory.FactoryStorageFilled -= ShowFactoryStorageNotification;
			_factory.FactoryStorageEmptied -= HideFactoryStorageNotification;
			_factory.HeroStorageEmptied -= ShowHeroStorageNotification;
			_factory.HeroStorageFilled -= HideHeroStorageNotification;
		}

		private void ShowFactoryStorageNotification() => Show(_factoryStorageNotification);
		private void ShowHeroStorageNotification() => Show(_heroStorageNotification);
		private void HideFactoryStorageNotification() => Hide(_factoryStorageNotification);
		private void HideHeroStorageNotification() => Hide(_heroStorageNotification);

		private void Show(TextMeshProUGUI field)
		{
			if(field == null) return;
			
			field.alpha = 1;
			field.transform.DOPunchScale(Vector3.one * _settings.Strength, _settings.Duration, 1);
		}

		private void Hide(TextMeshProUGUI field)
		{
			if(field == null) return;
			
			field.DOFade(0, 0.15f);
		}
	}
}