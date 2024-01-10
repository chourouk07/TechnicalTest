using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "ScriptableObjects/SwordSO", order = 1)]
public class SwordSO : ScriptableObject
{
    public AnimatorOverrideController animatorOV;
    public float damage;
}
