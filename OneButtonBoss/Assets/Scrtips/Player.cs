using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator playerAnimator; //reference for animator

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
            playerAnimator.SetBool("isAttacking", true);
        }
        else
        {
            playerAnimator.SetBool("isAttacking", false);
        }
    }
}
