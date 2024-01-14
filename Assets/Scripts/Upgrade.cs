using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Upgrade
{
    public string Name;
    public int Cost;
    public Sprite ImageObj;
    public int Quantity;
    [HideInInspector] public GameObject ItemRef;
}
