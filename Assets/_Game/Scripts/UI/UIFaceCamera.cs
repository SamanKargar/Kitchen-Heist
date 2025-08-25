using System;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class UIFaceCamera : MonoBehaviour {
        [SerializeField] private UILookAtMode lookAtMode;

        private Camera _camera;

        private void Awake() {
            _camera = Camera.main;
        }

        private void LateUpdate() {
            switch (lookAtMode) {
                case UILookAtMode.LookAt:
                    transform.LookAt(_camera.transform);
                    break;
                case UILookAtMode.LookAtInverted:
                    Vector3 dirFromCamera = transform.position - _camera.transform.position;
                    transform.LookAt(transform.position + dirFromCamera);
                    break;
                case UILookAtMode.CameraForward:
                    transform.forward = _camera.transform.forward;
                    break;
                case UILookAtMode.CameraForwardInverted:
                    transform.forward = -_camera.transform.forward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}