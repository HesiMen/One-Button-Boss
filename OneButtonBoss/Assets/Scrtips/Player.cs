using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator playerAnimator; //reference for animator
    public delegate void OnPlayerAttack();
    public static OnPlayerAttack playerAttack;
    public bool isPlayerAttacking;
    [SerializeField] float slashTiming = 1f;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Slash") > 0f)
        {
            Slash();
        }
    }


    private void Slash()
    {
            if (!isPlayerAttacking)
            {
                isPlayerAttacking = true;
                playerAttack.Invoke();
            }
                
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
}
