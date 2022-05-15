using System;
using UnityEngine;

namespace CodeBase.Logic
{
	[RequireComponent(typeof(Collider))]
	public class TriggerListener : MonoBehaviour
	{
		public event Action<Collider> Entered;
		public event Action<Collider> Canceled;

		private void OnTriggerEnter(Collider other) => Entered?.Invoke(other);
		private void OnTriggerExit(Collider other) => Canceled?.Invoke(other);
	}
}