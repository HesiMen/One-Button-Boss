using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DuelManager : MonoBehaviour
{
    public List<EnemyAI> allHeros;
    [SerializeField] Player player;
    bool playerAttacking;
    [SerializeField] float timeWindow = 0.5f;
    Vector2 playerPosition;
    Vector2 enemyPosition;
    Quaternion playerRotation;
    Quaternion enemyRotation;
    [SerializeField] private FullScreenEffectManager fullScreenEffectManager;
    [SerializeField] private CameraEffectManager cameraEffectManager;
    
    [Header("Audio Settings")]
    [SerializeField] AudioClip clashSound;
    [SerializeField] float clashVolume = 1f;
    [SerializeField] AudioClip bloodSound;
    [SerializeField] float bloodVolume = 1f;

    [SerializeField] GameObject bloodSplatter;

    private void Awake()
    {
        Player.playerAttack += PlayerAttackFlag;
    }


    private void PlayerAttackFlag()
    {
        fullScreenEffectManager.PlayAnimateMaterial();
        cameraEffectManager.Zoom();
        cameraEffectManager.ShakeCamera();
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
        AudioSource.PlayClipAtPoint(clashSound, Camera.main.transform.position, clashVolume);
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
            //Swap positions on Attack
            player.transform.position = enemy.transform.position;
            enemy.transform.position = playerPosition;
            yield return new WaitForSeconds(1f);

            //Kill enemy and spawn effects and play Audio
            enemy.Kill();
            Instantiate(bloodSplatter, enemy.transform.position, enemy.transform.rotation);
            enemy.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(bloodSound, Camera.main.transform.position, bloodVolume);
            yield return new WaitForSeconds(0.5f);

            //Disengage fight, count player win, and continue
            fullScreenEffectManager.PlayDisengageFight();
            player.MovePlayerBackToOrigin();
            Debug.Log("Player Won");
            enemy.myAtk -= OnAtk;
            playerAttacking = false;
        }
        else
        {
            Debug.Log("Enemy Won");
            AudioSource.PlayClipAtPoint(bloodSound, Camera.main.transform.position, bloodVolume);
            Instantiate(bloodSplatter, player.transform.position, player.transform.rotation);
            player.gameObject.SetActive(false);
        }
    }
}
