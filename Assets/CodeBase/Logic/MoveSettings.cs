using DG.Tweening;
using UnityEngine;

namespace CodeBase.Logic
{
	[CreateAssetMenu(fileName = "MoveSettings", menuName = "Settings/Move")]
	public class MoveSettings : ScriptableObject
	{
		public Ease EaseMode;
		public float ResourceMoveDuration;
	}
}