using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "OneButtonBoss/WeaponSO", order = 0)]
public class WeaponSO : ScriptableObject
{
    public List<GameObject> closeRangeWeaponCollection;
    public List<GameObject> mediumRangeWeaponCollection;
    public List<GameObject> longRangeWeaponCollection;
}
