using _Game.Scripts.Managers;
using Unity.Cinemachine;
using UnityEngine;

namespace _Game.Scripts.Characters.PlayerCharacter {
    public class PlayerCamera : MonoBehaviour {
        [SerializeField] private CinemachineCamera fpsCamera;
        [SerializeField] private Transform followTargetTransform;
        [SerializeField] private float rotationRate = 10f;
        [SerializeField] private float topClamp = 90f;
        [SerializeField] private float bottomClamp = -90f;

        private float _cinemachineTargetPitch;
        private float _cinemachineTargetYaw;
        private bool _isFpsActive;

        private Vector2 _mouseInput;

        private void Awake() {
            fpsCamera.enabled = false;
        }

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnLookActionEvent += InputEvents_OnLookActionEvent;
            
            GameEventsManager.Instance.MiscEvents.OnEnterHidingSpotEvent += MiscEvents_OnEnterHidingSpotEvent;
            GameEventsManager.Instance.MiscEvents.OnExitHidingSpotEvent += MiscEvents_OnExitHidingSpotEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnLookActionEvent -= InputEvents_OnLookActionEvent;
            
            GameEventsManager.Instance.MiscEvents.OnEnterHidingSpotEvent -= MiscEvents_OnEnterHidingSpotEvent;
            GameEventsManager.Instance.MiscEvents.OnExitHidingSpotEvent -= MiscEvents_OnExitHidingSpotEvent;
        }
        
        private void MiscEvents_OnExitHidingSpotEvent() {
            _isFpsActive = false;
            fpsCamera.enabled = false;
        }

        private void MiscEvents_OnEnterHidingSpotEvent() {
            _isFpsActive = true;
            fpsCamera.enabled = true;
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
            if (_isFpsActive) {
                _cinemachineTargetYaw = UpdateRotation(_cinemachineTargetYaw, _mouseInput.x, float.MinValue, float.MaxValue, false);
                ApplyRotation(0f, _cinemachineTargetYaw);
            }
            else {
                _cinemachineTargetPitch = UpdateRotation(_cinemachineTargetPitch, _mouseInput.y, bottomClamp, topClamp, true);
                _cinemachineTargetYaw = UpdateRotation(_cinemachineTargetYaw, _mouseInput.x, float.MinValue, float.MaxValue, false);
                ApplyRotation(_cinemachineTargetPitch, _cinemachineTargetYaw);
            }
        }
    }
}