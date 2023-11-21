using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    public List<EnemyAI> allHeros;
    [SerializeField] Player player;
    bool playerAttacking;

    private void Awake()
    {
        Player.playerAttack += PlayerAttackFlag;
    }


    private void PlayerAttackFlag()
    {
        if (!playerAttacking)
        {
            playerAttacking = true;
        }
            
    }

    public void SubscribeEnemy(EnemyAI enemy)
    {
        enemy.myAtk += OnAtk;
        allHeros.Add(enemy);
    }

    
   void Update()
    {
        if (playerAttacking)
        {
            Debug.Log("Player has attacked");
            playerAttacking = false;
        }
    }



    private void OnAtk(bool atkNow, EnemyAI enemy)
    {
        Debug.Log("Enemy has attacked");

        if (playerAttacking)
        {
            Debug.Log("Player has attacked");
        }

        //if killed 
        //enemy.myAtk -= OnAtk;
        //enemy.gameObject.SetActive(false);
    }
}
