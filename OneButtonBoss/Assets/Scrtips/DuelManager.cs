using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelManager : MonoBehaviour
{
    public List<EnemyAI> allHeros;
    [SerializeField] Player player;
    bool playerAttacking;
    [SerializeField] float timeWindow = 0.5f;
    bool battleResult = false;
    Vector2 playerPosition;
    Vector2 enemyPosition;
    Quaternion playerRotation;
    Quaternion enemyRotation;

    private void Awake()
    {
        Player.playerAttack += PlayerAttackFlag;
    }


    private void PlayerAttackFlag()
    {
        playerAttacking = player.isPlayerAttacking;
            
    }

    public void SubscribeEnemy(EnemyAI enemy)
    {
        enemy.myAtk += OnAtk;
        allHeros.Add(enemy);
    }



    private void OnAtk(bool atkNow, EnemyAI enemy)
    {
        Debug.Log("Enemy has attacked");

        StartCoroutine(SlashWindow(enemy));

        //enemy.transform.position = playerPosition;
        
        
        //if killed 
        //enemy.myAtk -= OnAtk;
    }


    /*
     * Timer that controls the amount of time a player 
     * can attack before the battle result is called.
     */
    IEnumerator SlashWindow(EnemyAI enemy)
    {
        yield return new WaitForSeconds(timeWindow);
        if (playerAttacking)
        {
            battleResult = true;
            Debug.Log("Player Won");
            enemy.myAtk -= OnAtk;
            enemy.gameObject.SetActive(false);
            playerAttacking = false;
            player.transform.position = playerPosition;
            player.transform.rotation = playerRotation;
        }
        else
        {
            battleResult = false;
            Debug.Log("Enemy Won");
            player.gameObject.SetActive(false);
        }
    }
}
