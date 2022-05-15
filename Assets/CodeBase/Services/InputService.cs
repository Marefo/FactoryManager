using UnityEngine;

namespace CodeBase.Services
{
	public class InputService : MonoBehaviour
	{
		public Vector2 JoystickInput => new Vector2(_joystick.Horizontal, _joystick.Vertical);
		public Vector2 Direction => _joystick.Direction;
	
		[SerializeField] private Joystick _joystick;
	}
}