using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform _player, _weaponContainer;
    //Input_Manager _playerInput;
    [SerializeField] private float _pickupRange;
    private Rigidbody _rb;
    private BoxCollider _collider;
    [SerializeField] private bool _isEquiped;
    static bool _isSlotFull;
/* bool _isPickPressed;
    bool _isDropPressed;*/
    private void Awake()
    {
        //_playerInput = new Input_Manager();
        _collider = GetComponent<BoxCollider>();
        _rb= GetComponent<Rigidbody>();
        if (_isEquiped)
        {
            _isSlotFull= true;
        }
/*
        _playerInput.Player.PickUp.started += OnPick;
        _playerInput.Player.Run.canceled += OnPick;
        _playerInput.Player.DropDown.started += OnDrop;
        _playerInput.Player.DropDown.canceled += OnDrop;*/
    }

/*    void OnPick(InputAction.CallbackContext ctx)
    {
        _isPickPressed = ctx.ReadValueAsButton();
    }

    void OnDrop(InputAction.CallbackContext ctx)
    {
        _isDropPressed = ctx.ReadValueAsButton();
    }*/
    private void Update()
    {
        Vector3 distanceToPlayer = _player.position - transform.position;
        if (!_isEquiped && distanceToPlayer.magnitude <= _pickupRange && Keyboard.current.eKey.wasPressedThisFrame)
            PickUp();
        if (_isEquiped && Keyboard.current.qKey.wasPressedThisFrame)
            Drop();
        
    }
    private void PickUp()
    {
        _isEquiped = true;
        _isSlotFull = true;
        _collider.isTrigger = true;
        _rb.useGravity = false;
        transform.SetParent(_weaponContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

    }
    private void Drop() 
    {
        _isEquiped = false;
        _isSlotFull = false;
        transform.SetParent(null);
        _collider.isTrigger= false;
        _rb.useGravity = true;

    }

 /*   private void OnEnable()
    {
        _playerInput.Player.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }*/
}
