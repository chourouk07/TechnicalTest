using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform _player, _weaponContainer;
    [SerializeField] private float _pickupRange;
    private Rigidbody _rb;
    private BoxCollider _collider;
    [SerializeField] private bool _isEquiped;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rb= GetComponent<Rigidbody>();
    }

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
        if (_weaponContainer.childCount == 0)
        {
            _isEquiped = true;
            _collider.isTrigger = true;
            _rb.useGravity = false;
            transform.SetParent(_weaponContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;
        }

    }
    private void Drop() 
    {
        _isEquiped = false;
        transform.SetParent(null);
        _collider.isTrigger= false;
        _rb.useGravity = true;

    }
}