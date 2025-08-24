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
        
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private float rotationSpeed = 5.5f;
        
        private bool _isGrounded;
        private float _verticalVelocity;
        private float _turnSmoothVelocity;
        private Vector2 _movementInput;
        
        private Camera _camera;
        private CharacterController _controller;
        
        private const float GRAVITY = -9.81f;

        private void Awake() {
            _camera = Camera.main;
            _controller = GetComponent<CharacterController>();
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
            GroundMovement();
            Turn();
        }

        private void GroundMovement() {
            Vector3 direction = new Vector3(_movementInput.x, 0, _movementInput.y);
            
            direction = _camera.transform.TransformDirection(direction);
            direction.y = CalculateVerticalVelocity();

            _controller.Move(direction * (movementSpeed * Time.deltaTime));

        }

        private void Turn() {
            if (!(Mathf.Abs(_movementInput.x) > 0) && !(Mathf.Abs(_movementInput.y) > 0)) return;
            
            Vector3 currentLookDirection = _controller.velocity.normalized;
            currentLookDirection.y = 0;

            currentLookDirection.Normalize();
                
            Quaternion targetRotation = Quaternion.LookRotation(currentLookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void OnDrawGizmosSelected() {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = _isGrounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundRadius);

        }
    }
}