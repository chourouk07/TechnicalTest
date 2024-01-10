using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttackController : MonoBehaviour, IWeapon
{
    string _weaponName;
    public string WeaponName => _weaponName;
    public bool ActivateWeapon(WeaponSwitch weaponSwitch)
    {
        Debug.Log("Gun Activated");
        return true;
    }
}
