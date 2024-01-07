using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region References
    Input_Manager _playerInput;
    CharacterController _characterController;
    [SerializeField] Animator _animator;
    #endregion
    #region player movement variables
    Vector2 _movementInput;
    Vector3 _movement;
    Vector3 _runMovement;
    Vector3 _jumpMovement;
    bool _isRunPressed;
    bool _isMovementPressed;
    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _runSpeedMultiplier = 2.0f;
    float _rotationSpeed = 15f;
    //jumping 
    bool _isJumpPressed= false;
    bool _isJumping = false;
    [SerializeField] float _jumpHeight = 5f;
    bool _isjumpAnimation= false;
    bool _canDoubleJump;
    #endregion
    #region Animation
    int _isWalkingHash;
    int _isRunningHash;
    int _isJumpingHash;
    int _isDoubleJumpingHash;
    #endregion
    private void Awake()
    {
        
        _playerInput = new Input_Manager();
        _characterController = GetComponent<CharacterController>();
        //set the player input callbacks
        _playerInput.Player.Move.started += OnMovementInput;
        _playerInput.Player.Move.canceled+= OnMovementInput;
        _playerInput.Player.Move.performed+= OnMovementInput;
        _playerInput.Player.Run.started += OnRun;
        _playerInput.Player.Run.canceled+= OnRun;
        _playerInput.Player.Jump.started += OnJump;
        _playerInput.Player.Jump.canceled += OnJump;

        //animation hash
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isDoubleJumpingHash = Animator.StringToHash("isDoubleJumping");
    }

    void OnRun(InputAction.CallbackContext ctx)
    {
        _isRunPressed = ctx.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
            _isJumpPressed = ctx.ReadValueAsButton();
    }

    void HandleRotation()
    {
        Vector3 lookAtDirection;

        lookAtDirection.x = _movement.x;
        lookAtDirection.y = 0f;
        lookAtDirection.z = _movement.z;

        Quaternion rotation = transform.rotation;

        if (_isMovementPressed )
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);
            transform.rotation = Quaternion.Slerp(rotation, targetRotation, _rotationSpeed* Time.deltaTime);
        }
    }
        
    void OnMovementInput(InputAction.CallbackContext ctx)
    {
        _movementInput = ctx.ReadValue<Vector2>();
        _movement.x = _movementInput.x*_movementSpeed;
        _movement.z = _movementInput.y * _movementSpeed;
        _runMovement.x = _movement.x * _runSpeedMultiplier;
        _runMovement.z = _movement.z * _runSpeedMultiplier;
        _isMovementPressed = _movementInput.x != 0 || _movementInput.y != 0;
    }

    void HandleGravity()
    {
        bool isFalling = _movement.y <= 0f;
        float fallMultiplier = 3f;
        if (_characterController.isGrounded)
        {
            if (_isjumpAnimation)
            {
                _animator.SetBool(_isJumpingHash, false);
                _isjumpAnimation= false;
            }
            _animator.SetBool (_isDoubleJumpingHash, false);
            float groundedGravity = -.05f;
            _movement.y = groundedGravity;
            _runMovement.y = groundedGravity;
            if (!_isJumpPressed)
            {
                _canDoubleJump = false;
            }
        }
        else if (isFalling)
        {
            float gravity = -9.81f * fallMultiplier;
            _movement.y += gravity * Time.deltaTime;
            _runMovement.y += gravity * Time.deltaTime;
        }
        else
        {
            float gravity = -9.81f; 
            _movement.y += gravity * Time.deltaTime;
            _runMovement.y += gravity * Time.deltaTime;
        }
    }
    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool isRunning = _animator.GetBool(_isRunningHash);

        if (_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }

        if ((_isMovementPressed && _isRunPressed)   && !isRunning)
        {
            _animator.SetBool(_isRunningHash, true);
        }
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
        {
            _animator.SetBool(_isRunningHash, false);
        }
    }

    void HandleJump()
    {
        if (_isJumpPressed)
        {
            if (_characterController.isGrounded && !_isJumping)
            {
                _animator.SetBool(_isJumpingHash, true);
                _isjumpAnimation = true;
                _isJumping = true;
                float jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics.gravity.y));
                _movement.y = jumpVelocity;
                _runMovement.y = jumpVelocity;
                _canDoubleJump = true;
            }
            if (!_isJumping && _canDoubleJump)
            {
                _animator.SetBool(_isDoubleJumpingHash, true);
                _isJumping = true;
                float jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics.gravity.y));
                _movement.y = jumpVelocity;
                _canDoubleJump = false;
            }
        }
        if (!_isJumpPressed && _isJumping)
        {
            _isJumping = false;
        }
    }


    private void Update()
    {
        
        HandleRotation();
        HandleAnimation();

        
        if (_isRunPressed)
        {
            _characterController.Move(_runMovement * Time.deltaTime);
        }
        else
        {
            _characterController.Move(_movement * Time.deltaTime);
        }

        HandleGravity();
        HandleJump();
    }
    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }
}
