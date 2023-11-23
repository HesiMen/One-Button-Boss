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
    [SerializeField] private FullScreenEffectManager fullScreenEffectManager;

    private void Awake()
    {
        Player.playerAttack += PlayerAttackFlag;
    }


    private void PlayerAttackFlag()
    {
        fullScreenEffectManager.PlayAnimateMaterial();
        playerAttacking = player.isPlayerAttacking;
        player.rotationTarget = null;
    }

    public void SubscribeEnemy(EnemyAI enemy)
    {
        fullScreenEffectManager.PlayEngageFight();
        enemy.myAtk += OnAtk;
        player.rotationTarget = enemy.transform;
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
        // Won't continue Coroutine until player attacks or time window ends
        float currentTime = 0;
        while(!playerAttacking && currentTime <= timeWindow)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        
        if (playerAttacking)
        {
            //Swap positions
            player.transform.position = enemy.transform.position;
            enemy.transform.position = playerPosition;
            yield return new WaitForSeconds(1f);
            player.MovePlayerBackToOrigin();
            battleResult = true;
            Debug.Log("Player Won");
            enemy.myAtk -= OnAtk;
            enemy.gameObject.SetActive(false);
            playerAttacking = false;
            //player.transform.position = playerPosition;
            //player.transform.rotation = playerRotation;
        }
        else
        {
            battleResult = false;
            Debug.Log("Enemy Won");
            player.gameObject.SetActive(false);
        }
    }
}
