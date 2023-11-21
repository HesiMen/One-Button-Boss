using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator playerAnimator; //reference for animator
    public delegate void OnPlayerAttack();
    public static OnPlayerAttack playerAttack;
    bool isPlayerAttacking;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Slash();
    }


    private void Slash()
    {
        if (Input.GetAxis("Slash") > 0f)
        {
            if (!isPlayerAttacking)
            {
                playerAttack.Invoke();
                isPlayerAttacking = true;
            }
                
            playerAnimator.SetBool("isAttacking", true);
        }
        else
        {
            isPlayerAttacking = false;
            //playerAttack.Invoke();
            playerAnimator.SetBool("isAttacking", false);
        }
    }
}
