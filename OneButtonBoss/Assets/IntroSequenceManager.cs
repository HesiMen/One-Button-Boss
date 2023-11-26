using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;

public class IntroSequenceManager : MonoBehaviour
{
    [SerializeField] private bool showIntro;
    [SerializeField] private Player player;
    [SerializeField] private GameObject introScreen;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("IntroAnimation")]
    [SerializeField] private float playerWalkToCenter;
    [SerializeField] private Animation introScreenAnimComponent;
    [SerializeField] private AnimationClip animInAnimClip, animOutAnimClip, idleAnimClip;

    private void Awake()
    {
        //override value on enemy spawner
        if(showIntro)
            enemySpawner.onStart = false;
    }

    private void Start()
    {
        if(showIntro)
        {
            player.transform.DOMove(Vector3.zero, playerWalkToCenter).SetEase(Ease.InOutSine).OnComplete(() => ShowIntro());
        }
    }

    
    private void StartGame()
    {
        enemySpawner.StartSpawningHeros();
    }

    private void ShowIntro()
    {
        PlayAnimation(animInAnimClip);
        StartCoroutine(WaitForPlayerInput());
    }

    IEnumerator WaitForPlayerInput()
    {
        //wait for animation to finish
        yield return new WaitForSeconds(animInAnimClip.length);
        player.readyToStart = true;
        bool playerPressedSpace = false;
        while(!playerPressedSpace)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                playerPressedSpace = true;
            }
            yield return null;
        }

        // Play Slash Sequence to then start game
        PlayAnimation(animOutAnimClip);
        yield return new WaitForSeconds(animOutAnimClip.length);
        StartGame();
    }

    private void PlayAnimation(AnimationClip clip, bool queue = false)
    {
        introScreenAnimComponent.clip = clip;

        if(queue)
        {
            introScreenAnimComponent.PlayQueued(clip.name, QueueMode.CompleteOthers);
            return;
        }
        
        introScreenAnimComponent.Play();

    }
}
