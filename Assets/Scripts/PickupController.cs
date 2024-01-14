using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PickupController : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string weaponContainerName = "WeaponContainer";
    [SerializeField] private float _pickupRange;
    private Rigidbody _rb;
    private BoxCollider _collider;
    [SerializeField] private bool _isEquipped;

    [SerializeField] private Transform _player;
    [SerializeField] private Transform _weaponContainer;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // Find player and weapon container at runtime
        _player = GameObject.FindGameObjectWithTag(playerTag).transform;
        _weaponContainer = FindDeepChild(_player, weaponContainerName);

        if (_player == null || _weaponContainer == null)
        {
            Debug.LogError("Player or WeaponContainer not found!");
        }
    }

    private void Update()
    {
        Vector3 distanceToPlayer = _player.position - transform.position;
        if (!_isEquipped && distanceToPlayer.magnitude <= _pickupRange && Keyboard.current.eKey.wasPressedThisFrame)
            PickUp();
        if (_isEquipped && Keyboard.current.qKey.wasPressedThisFrame)
            Drop();
    }

    private void PickUp()
    {
        if (_weaponContainer.childCount == 0)
        {
            _isEquipped = true;
            _collider.isTrigger = true;
            _rb.useGravity = false;
            transform.SetParent(_weaponContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            //transform.localScale = Vector3.one;
        }
    }

    private void Drop()
    {
        _isEquipped = false;
        transform.SetParent(null);
        _collider.isTrigger = false;
        _rb.useGravity = true;
    }

    private Transform FindDeepChild(Transform parent, string childName)
    {
        Transform result = null;

        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                result = child;
                break;
            }
            else
            {
                result = FindDeepChild(child, childName);
                if (result != null)
                    break;
            }
        }

        return result;
    }
}
