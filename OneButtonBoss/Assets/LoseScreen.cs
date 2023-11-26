using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Animation animComp;
    [SerializeField] public AnimationClip animClipShow, animClipHide;

    public void ShowLose()
    {
        animComp.clip = animClipShow;
        animComp.Play();
    }

    public void HideLose()
    {
        animComp.clip = animClipHide;
        animComp.Play();
    }
}
