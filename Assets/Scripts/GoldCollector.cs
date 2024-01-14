using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GoldCollector : MonoBehaviour
{
    public UnityEvent goldCollect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            goldCollect.Invoke();
            Destroy(gameObject);
        }
    }
    public void CollectGold()
    {
        LevelManager.instance.ChangeGold(10);
    }
}
