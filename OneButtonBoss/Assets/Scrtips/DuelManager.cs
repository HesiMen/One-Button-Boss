using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class DuelManager : MonoBehaviour
{
    public List<EnemyAI> allHeros;
    private EnemyAI currentHero;
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

    [SerializeField] AudioSource sfx;

    public bool playerDead = false;


    /// Battle Tracker
    [SerializeField] bool allowEarlyHit = true;
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    private float playerAttackTime = -1f; // Time when the player attacked
    private float enemyAttackTime = -1f; // Time when the enemy attacked

    public UnityEvent wonEvent;
    public UnityEvent loseEvent;
    public UnityEvent tooEarlyLoseEvent;
    public UnityEvent tooEarlyWarmUpEvent;
    void BattleStarts(System.Func<IEnumerator> currentBattleFunc)
    {
        RecordBattle(currentBattleFunc);
    }

    void RecordBattle(System.Func<IEnumerator> currentBattleFunc)
    {
        IEnumerator coroutine = currentBattleFunc.Invoke();
        Coroutine coroutineReference = StartCoroutine(coroutine);
        activeCoroutines.Add(coroutineReference);
    }

    private IEnumerator MyBattle(EnemyAI enemy)
    {
        while (!playerDead || enemy.Alive)
        {


            if (!allowEarlyHit && playerAttacking)
            {
                Debug.Log("PLayer attacked early - Fail");
                tooEarlyLoseEvent.Invoke();
            }
            else if (allowEarlyHit && playerAttacking)
            {
                tooEarlyWarmUpEvent.Invoke();
                Debug.Log("PLayer attacked early - All Good");
            }

            
            yield return null;
        }
    }


    // Call this method to stop all active coroutines
    public void StopAllCoroutines()
    {
        foreach (var coroutine in activeCoroutines)
        {
            StopCoroutine(coroutine);
        }
        activeCoroutines.Clear();
    }




    private void Awake()
    {
        Player.playerAttack += PlayerAttackFlag;
    }


    private void PlayerAttackFlag()
    {
        playerAttackTime = Time.time;

        if (currentHero.currentState != EnemyAI.AIState.Attack)
            return;
        fullScreenEffectManager.PlayAnimateMaterial();
        cameraEffectManager.Zoom();
        cameraEffectManager.ShakeCamera();
        playerAttacking = player.isPlayerAttacking;
        player.rotationTarget = null;
    }

    public void SubscribeEnemy(EnemyAI enemy)
    {
        BattleStarts(() => MyBattle(enemy));
        playerAttackTime = -1f; 
        enemyAttackTime = -1f; 

        fullScreenEffectManager.PlayEngageFight();
        enemy.myAtk += OnAtk;
        player.rotationTarget = enemy.transform;
        allHeros.Add(enemy);
        currentHero = enemy;
    }


    void PlayRandomClash()
    {
        float randomPitch = Random.Range(0.85f, 1.15f);
        if (sfx != null)
        {
            sfx.pitch = randomPitch;
            sfx.PlayOneShot(clashSound, clashVolume);
        }
    }


    private void OnAtk(bool atkNow, EnemyAI enemy)
    {
        Debug.Log("Enemy has attacked");
        enemyAttackTime = Time.time;
        PlayRandomClash();
        fullScreenEffectManager.PlayExclamationPoint();
        StartCoroutine(SlashWindow(enemy));

    }



    private void CheckTimeBeforeEnemyAtk()
    {
        if (playerAttackTime < 0 || enemyAttackTime < 0)
        {
            Debug.LogError("Attack times not set properly");
            return;
        }

        float timeDifference = playerAttackTime - enemyAttackTime;
        if (timeDifference < 0)
        {
            Debug.Log($"Player attacked {Mathf.Abs(timeDifference)} seconds early.");
        }
        else
        {
            Debug.Log($"Player attacked {timeDifference} seconds late.");
        }

       
    }
    /*
     * Timer that controls the amount of time a player 
     * can attack before the battle result is called.
     */
    IEnumerator SlashWindow(EnemyAI enemy)
    {
        // Won't continue Coroutine until player attacks or time window ends
        float currentTime = 0;
        while (!playerAttacking && currentTime <= timeWindow)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }

        if (playerAttacking)
        {
            //Remove exclamationPoint
            fullScreenEffectManager.PlayRemoveExclamationPoint();

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
            wonEvent.Invoke();
            enemy.myAtk -= OnAtk;
            playerAttacking = false;
        }
        else
        {
            Debug.Log("Enemy Won");
            loseEvent.Invoke();
            fullScreenEffectManager.PlayRemoveExclamationPoint();
            AudioSource.PlayClipAtPoint(bloodSound, Camera.main.transform.position, bloodVolume);
            Instantiate(bloodSplatter, player.transform.position, player.transform.rotation);
            player.gameObject.SetActive(false);
        }

        CheckTimeBeforeEnemyAtk();
    }
}
