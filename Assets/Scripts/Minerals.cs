using UnityEngine;
using UnityEngine.Events;

public class Minerals : MonoBehaviour
{
    public UnityEvent MineralEvent;

    private void OnDisable()
    {
        MineralEvent.Invoke();
    }
}
