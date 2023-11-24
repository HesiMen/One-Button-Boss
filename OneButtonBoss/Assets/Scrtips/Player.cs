using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    Animator playerAnimator; //reference for animator
    public delegate void OnPlayerAttack();
    public static OnPlayerAttack playerAttack;
    public bool isPlayerAttacking;
    [SerializeField] private float slashTiming = 1f;
    [SerializeField] public Transform rotationTarget;
    [SerializeField] private float tweenToOriginTime;
    [SerializeField] private float rotationTime;

    [SerializeField] AudioClip swordSlash;
    [SerializeField] float slashVolume = 1f;
    [SerializeField] AudioSource slashSFX;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: remove this get component call and directly reference it by serializign the animator field
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Slash") > 0f)
            Slash();

        //Face target if it's defined
        if(rotationTarget)
            FaceEnemy(rotationTarget);
    }


    void PlayRandomSlash()
    {
        float randomPitch = Random.Range(1f, 2f);
        slashSFX.pitch = randomPitch;
        slashSFX.PlayOneShot(swordSlash, slashVolume);
    }

    private void Slash()
    {
            if (!isPlayerAttacking)
            {
                isPlayerAttacking = true;
                playerAttack.Invoke();
            }
            PlayRandomSlash();
            playerAnimator.SetBool("isAttacking", true);
            StartCoroutine(StopSlash());
    }


    /*
     * Timer that will automatically stop the player from slashing.
     * Prevents just holding it down / spamming.
     */
    IEnumerator StopSlash()
    {
        yield return new WaitForSeconds(slashTiming);
        isPlayerAttacking = false;
        playerAnimator.SetBool("isAttacking", false);
    }

    private void FaceEnemy(Transform enemyTransform)
    {
        Vector2 directionToTarget = enemyTransform.position - this.transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.DORotateQuaternion(rotation, rotationTime);
    }

    public void MovePlayerBackToOrigin()
    {
        transform.DOMove(new Vector3(0,0,0), tweenToOriginTime);
    }
}
