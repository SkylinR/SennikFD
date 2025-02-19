using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerConfig playerConfig;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Animator animator;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float cameraRotationMultiplier;

    CharacterController _characterController;
    Vector2 _moveVector;
    Vector2 _cameraVector;
    float _rotationY = 0f;
    Vector3 _velocity;

    void Awake() {
        _characterController = GetComponent<CharacterController>();

        InputController.PlayerInputActions.Player.Move.performed += ctx => _moveVector = ctx.ReadValue<Vector2>();
        InputController.PlayerInputActions.Player.Move.canceled += ctx => _moveVector = Vector2.zero;

        InputController.PlayerInputActions.Player.Look.performed += ctx => _cameraVector = ctx.ReadValue<Vector2>();
        InputController.PlayerInputActions.Player.Look.canceled += ctx => _cameraVector = Vector2.zero;

        cameraTransform = Camera.main.transform;
    }

    void Update() {
        HandleGravity();
        HandleMovement();
        HandleCamera();
    }

    public void SetRotationY(float rotationY) {
        if (PlayerOptions.CameraSensivity > 0) {
            _rotationY = rotationY / PlayerOptions.CameraSensivity;
        } else {
            _rotationY = rotationY;
        }
    }

    void HandleGravity() {

        if (!_characterController.isGrounded) {
            _velocity.y -= gravity * Time.deltaTime;
        } else {
            _velocity.y = 0;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    void HandleMovement() {
        float horizontal = _moveVector.x;
        float vertical = _moveVector.y;
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f) {
            Vector3 moveDir = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
            moveDir.y = 0f;
            moveDir = moveDir.normalized;
            _characterController.Move(moveDir * playerConfig.GetMovementSpeed() * Time.deltaTime);

            if (audioSource.isPlaying == false) {
                audioSource.Play();
            }
            animator.SetBool("IsMoving", true);
        } else {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
            animator.SetBool("IsMoving", false);
        }
    }

    void HandleCamera() {
        float mouseX = _cameraVector.x;
        _rotationY += mouseX;
        
        transform.rotation = Quaternion.Euler(0f, _rotationY * cameraRotationMultiplier * PlayerOptions.CameraSensivity, 0f);
    }
}
