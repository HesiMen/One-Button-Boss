using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dificulty", menuName = "OneButtonBoss/DificultType", order = 2)]
public class DificultySO : ScriptableObject
{
    public enum TypesOfEnemy { Close, Medium, Both }
    public TypesOfEnemy typesOfEnemy;
    public Vector2 timeSpawnRange = new Vector2(5f, 7f);
    public bool shouldZigZag = false;
    public float zigZagWidth = 1.0f;
    public int zigZagCount = 3;
}
