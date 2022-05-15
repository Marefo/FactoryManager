using System;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Hero
{
	public class HeroMovement : MonoBehaviour
	{
		public event Action Stopped;

		public bool IsMoving => _handledInput != Vector3.zero;
		
		[SerializeField] private InputService _inputService;
		[SerializeField] private Transform _model;
		[Space(10)]
		[SerializeField] private float _moveSpeed;

		private Vector3 _lastHandledInput;
		private Vector3 _handledInput;
		
		private void Update()
		{
			HandleInput();
			Move();
			Rotate();
		}

		private void HandleInput() => 
			_handledInput = new Vector3(_inputService.JoystickInput.x, 0, _inputService.JoystickInput.y);

		private void Rotate()
		{
			if(_handledInput == Vector3.zero) return;
			
			Quaternion rotation = Quaternion.LookRotation(_handledInput);
			_model.rotation = rotation;
		}

		private void Move()
		{
			if(_lastHandledInput != Vector3.zero && _handledInput == Vector3.zero)
				Stopped?.Invoke();
			
			transform.position += _handledInput * (_moveSpeed * Time.deltaTime);
			_lastHandledInput = _handledInput;
		}
	}
}