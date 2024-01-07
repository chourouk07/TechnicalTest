using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickupController : MonoBehaviour
{
    [SerializeField] private Transform _player, _weaponContainer;
    [SerializeField] private float _pickupRange;
    private Rigidbody _rb;
    private BoxCollider _collider;
    [SerializeField] private bool _isEquiped;
    static bool _isSlotFull;

    private void Start()
    {
        _collider= GetComponent<BoxCollider>();
        _rb= GetComponent<Rigidbody>();
        if (_isEquiped)
        {
            _isSlotFull= true;
        }
    }
    private void Update()
    {
        Vector3 distanceToPlayer = _player.position - transform.position;
        if (!_isEquiped && distanceToPlayer.magnitude <= _pickupRange && Input.GetKeyDown(KeyCode.E))
         PickUp();
        if (_isEquiped && Input.GetKeyDown(KeyCode.Q))
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
}
