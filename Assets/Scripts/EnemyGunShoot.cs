using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunShoot : MonoBehaviour
{
    [SerializeField] Transform _shootingPoint;
    [SerializeField] GameObject _bulletPrefab;
    public void OnShoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _shootingPoint.position, _shootingPoint.rotation);
    }
}
