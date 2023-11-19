using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    public List<EnemyAI> allHeros;




    private void Start()
    {
        foreach (var hero in allHeros)
        {
            if (hero != null)
            {
                hero.myAtk += OnAtk;
            }
        }
    }

    private void OnAtk(bool atkNow, EnemyAI enemy)
    {
        Debug.Log("Enemy has attacked");

        //if killed 
        enemy.gameObject.SetActive(false);
    }
}
