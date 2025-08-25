using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Characters.PlayerCharacter {
    public class PlayerCamera : MonoBehaviour {
        [SerializeField] private Transform followTargetTransform;
        [SerializeField] private float rotationRate = 10f;
        [SerializeField] private float topClamp = 90f;
        [SerializeField] private float bottomClamp = -90f;

        private float _cinemachineTargetPitch;
        private float _cinemachineTargetYaw;

        private Vector2 _mouseInput;

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnLookActionEvent += InputEvents_OnLookActionEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnLookActionEvent -= InputEvents_OnLookActionEvent;
        }

        private void InputEvents_OnLookActionEvent(Vector2 inputValue) {
            _mouseInput = inputValue * rotationRate * Time.deltaTime;
        }

        private void LateUpdate() {
            HandleCameraMovement();
        }

        private float UpdateRotation(float currentRotation, float input, float min, float max, bool isXAxis) {
            currentRotation += isXAxis ? -input : input;
            return Mathf.Clamp(currentRotation, min, max);
        }
        
        private void ApplyRotation(float pitch, float yaw) {
            followTargetTransform.rotation = Quaternion.Euler(pitch, yaw, followTargetTransform.eulerAngles.z);
        }

        private void HandleCameraMovement() {
            _cinemachineTargetPitch = UpdateRotation(_cinemachineTargetPitch, _mouseInput.y, bottomClamp, topClamp, true);
            _cinemachineTargetYaw = UpdateRotation(_cinemachineTargetYaw, _mouseInput.x, float.MinValue, float.MaxValue, false);
            
            ApplyRotation(_cinemachineTargetPitch, _cinemachineTargetYaw);
        }
    }
}