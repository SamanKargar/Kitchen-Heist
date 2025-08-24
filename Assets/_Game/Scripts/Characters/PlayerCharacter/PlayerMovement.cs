using _Game.Scripts.Characters.Core;
using _Game.Scripts.Managers;
using UnityEngine;

namespace _Game.Scripts.Characters.PlayerCharacter {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour {
        [Header("Ground Check & Gravity")] [Space(5)]
        
        [SerializeField] private float groundedOffset;
        [SerializeField] private float groundRadius = 0.28f;
        [SerializeField] private LayerMask groundLayers;
        
        [Space(5)] [Header("Movement")] [Space(5)]
        
        [SerializeField] private float maxMovementSpeed = 3f;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float deceleration = 6f;
        [SerializeField] private float rotationSpeed = 7.5f;
        
        private bool _isGrounded;
        private float _verticalVelocity;
        private float _turnSmoothVelocity;
        private float _currentSpeed;
        private Vector2 _movementInput;
        
        private Camera _camera;
        private CharacterController _characterController;
        private AnimationController _animationController;
        
        private const float GRAVITY = -9.81f;

        private void Awake() {
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            _animationController = GetComponentInChildren<AnimationController>();
        }

        private void OnEnable() {
            GameEventsManager.Instance.InputEvents.OnMoveActionEvent += InputEvents_OnMoveActionEvent;
        }

        private void OnDisable() {
            GameEventsManager.Instance.InputEvents.OnMoveActionEvent -= InputEvents_OnMoveActionEvent;
        }
        
        private void InputEvents_OnMoveActionEvent(Vector2 inputValue) {
            _movementInput = inputValue;
        }

        private void Update() {
            GroundCheck();
            HandleMovement();
        }

        private void GroundCheck() {
            Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePos, groundRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private float CalculateVerticalVelocity() {
            if (_isGrounded) {
                _verticalVelocity = -2f;
            }
            else {
                _verticalVelocity += GRAVITY * Time.deltaTime;
            }

            return _verticalVelocity;
        }

        private void HandleMovement() {
            AdjustSpeed();
            GroundMovement();
            Turn();
        }
        
        private void AdjustSpeed() {
            _currentSpeed = _movementInput.magnitude > 0.1f ? 
                Mathf.MoveTowards(_currentSpeed, maxMovementSpeed, acceleration * Time.deltaTime) : 
                Mathf.MoveTowards(_currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        private void GroundMovement() {
            Vector3 direction = new Vector3(_movementInput.x, 0, _movementInput.y);
            direction = _camera.transform.TransformDirection(direction);
            direction.y = CalculateVerticalVelocity();

            _characterController.Move(direction.normalized * (_currentSpeed * Time.deltaTime));
            _animationController.PlayMoveAnimation(GetNormalizedSpeed());
        }

        private void Turn() {
            if (_movementInput.sqrMagnitude < 0.01f) return;

            Vector3 targetDirection = new Vector3(_movementInput.x, 0, _movementInput.y);
            targetDirection = _camera.transform.TransformDirection(targetDirection);
            targetDirection.y = 0;
            targetDirection.Normalize();

            if (targetDirection.sqrMagnitude > 0.01f) {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        private float GetNormalizedSpeed() {
            return Mathf.InverseLerp(0f, maxMovementSpeed, _currentSpeed);
        }

        private void OnDrawGizmosSelected() {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = _isGrounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundRadius);

        }
    }
}