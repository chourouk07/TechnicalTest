using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponSwitch : MonoBehaviour
{
    Input_Manager _playerInput;
    [SerializeField] private int _selectedWeapon = 0;
    private float mouseScrollY;

    private void Awake()
    {
        _playerInput = new Input_Manager();
        _playerInput.Player.Switch.performed += x => mouseScrollY = x.ReadValue<float>();
        SelectWeapon();
    }
    private void Update()
    {
        int previousSelectedWeapon = _selectedWeapon;
        if (mouseScrollY > 0) {
            if (_selectedWeapon >= transform.childCount - 1)
                _selectedWeapon = 0;
            else
                _selectedWeapon++;
        }
        if (mouseScrollY< 0)
        {
            if (_selectedWeapon <= 0)
                _selectedWeapon = transform.childCount - 1;
            else
                _selectedWeapon--;
        }

        if (previousSelectedWeapon!= _selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == _selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    #region Enable-Disable
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
