using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttackController : MonoBehaviour, IWeapon
{
    Input_Manager _playerInput;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _player;
    [SerializeField] Transform _shootingPoint;
    [SerializeField] AnimatorOverrideController _animatorOverride;
    string _weaponName;
    bool _isAttackPressed;
    bool _isAimPressed;
    Vector2 _mouseDelta;
    [SerializeField] GameObject _crosshair;
    [SerializeField] GameObject _bulletPrefab;

    public string WeaponName => _weaponName;

    public bool ActivateWeapon(WeaponSwitch weaponSwitch)
    {
        Debug.Log("Gun Activated");
        return true;
    }

    private void Awake()
    {
        _playerInput = new Input_Manager();

        _playerInput.Player.Attack.started+= OnAttack;
        _playerInput.Player.Attack.canceled += OnAttack;

        _playerInput.Player.Aim.performed += OnAim;
        _playerInput.Player.Aim.canceled += OnAim;
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        _isAttackPressed = ctx.ReadValueAsButton();
    }

    void OnAim(InputAction.CallbackContext ctx)
    {
        _isAimPressed = ctx.ReadValueAsButton();
    }

    void MoveCrosshair()
    {
        _mouseDelta = _playerInput.Player.MouseDelta.ReadValue<Vector2>();
        float newYRotation = _player.transform.localRotation.eulerAngles.y + _mouseDelta.x * Time.deltaTime * 50f;
        newYRotation = Mathf.Repeat(newYRotation, 360f);
        if (newYRotation > 180f)
        {
            newYRotation -= 360f;
        }
        newYRotation = Mathf.Clamp(newYRotation, -360f, 360f);

        //_crosshair.transform.localRotation = Quaternion.Euler(0f, newYRotation, 0f);
        _player.transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
    }
    private void Update()
    {
        if (_isAimPressed)
        {
            _animator.SetBool("isAiming", true);
            _crosshair.SetActive(true);
            MoveCrosshair();
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _animator.SetTrigger("isShooting");
                GameObject bullet = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);
                
            }
        }
        else
        {
            _animator.SetBool("isAiming", false);
            //stop Aim Animation
            _crosshair.SetActive(false);
        }
        
    }

    #region Enable/Disable Input
    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }
    #endregion
}
