using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region References
    Input_Manager playerInput;
    CharacterController characterController;
    [SerializeField] Animator animator;
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
    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;
    int isDoubleJumpingHash;
    #endregion
    private void Awake()
    {
        
        playerInput = new Input_Manager();
        characterController = GetComponent<CharacterController>();
        //set the player input callbacks
        playerInput.Player.Move.started += OnMovementInput;
        playerInput.Player.Move.canceled+= OnMovementInput;
        playerInput.Player.Move.performed+= OnMovementInput;
        playerInput.Player.Run.started += OnRun;
        playerInput.Player.Run.canceled+= OnRun;
        playerInput.Player.Jump.started += OnJump;
        playerInput.Player.Jump.canceled += OnJump;

        //animation hash
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isDoubleJumpingHash = Animator.StringToHash("isDoubleJumping");
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
        Vector3 _lookAtDirection;

        _lookAtDirection.x = _movement.x;
        _lookAtDirection.y = 0f;
        _lookAtDirection.z = _movement.z;

        Quaternion rotation = transform.rotation;

        if (_isMovementPressed )
        {
            Quaternion targetRotation = Quaternion.LookRotation(_lookAtDirection);
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
        if (characterController.isGrounded)
        {
            if (_isjumpAnimation)
            {
                animator.SetBool(isJumpingHash, false);
                _isjumpAnimation= false;
            }
            animator.SetBool (isDoubleJumpingHash, false);
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
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (_isMovementPressed && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            animator.SetBool("isWalking", false);
        }

        if ((_isMovementPressed && _isRunPressed)   && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void HandleJump()
    {
        if (_isJumpPressed)
        {
            if (characterController.isGrounded && !_isJumping)
            {
                animator.SetBool(isJumpingHash, true);
                _isjumpAnimation = true;
                _isJumping = true;
                float jumpVelocity = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics.gravity.y));
                _movement.y = jumpVelocity;
                _runMovement.y = jumpVelocity;
                _canDoubleJump = true;
            }
            if (!_isJumping && _canDoubleJump)
            {
                animator.SetBool(isDoubleJumpingHash, true);
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
            characterController.Move(_runMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(_movement * Time.deltaTime);
        }

        HandleGravity();
        HandleJump();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
