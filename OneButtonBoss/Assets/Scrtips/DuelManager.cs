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
        //enemy.gameObject.SetActive(false);
    }
}
