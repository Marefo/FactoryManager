using UnityEngine;

namespace CodeBase.Logic
{
	[CreateAssetMenu(fileName = "PunchScaleEffectSettings", menuName = "Settings/PunchScaleEffect")]
	public class PunchScaleEffectSettings : ScriptableObject
	{
		public float Strength;
		public float Duration;
	}
}