using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    private bool _isDamaging = false;

    public bool IsDamaging()
    {
        return _isDamaging;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isDamaging = true;
        }
    }
}
