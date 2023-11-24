using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenEffectManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image effectImage;
    [SerializeField] private AnimationCurve effectCurve;
    [SerializeField] private float effectTime;
    [SerializeField] private float removeTime;

    [Header("Legacy Animation")]
    [SerializeField] private Animation animComponent;
    [SerializeField] private AnimationClip animClipFadeOut;
    [SerializeField] private AnimationClip animClipEngageFight;
    [SerializeField] private AnimationClip animClipDisengageFight;
    [SerializeField] private AnimationClip animClipExclamationPoint;

    private void PlayAnimation(AnimationClip clip)
    {
        animComponent.clip = clip;
        animComponent.PlayQueued(clip.name, QueueMode.CompleteOthers, PlayMode.StopAll);
    }

    private IEnumerator AnimateUIMaterial(float effectTime)
    {
        float currentTime = 0;
        canvasGroup.alpha = 1;
        //float currentRemoveTime = 0;
        while(currentTime <= effectTime)
        {
            currentTime += Time.deltaTime;
            effectImage.material.SetFloat("_Fill", effectCurve.Evaluate(currentTime/effectTime));
            yield return null;
        }
        PlayAnimation(animClipFadeOut);
    }

    public void ResetMaterialValue()
    {
        effectImage.material.SetFloat("_Fill", 0f);
    }

    public void PlayEngageFight()
    {
        PlayAnimation(animClipEngageFight);
    }

    public void PlayDisengageFight()
    {
        PlayAnimation(animClipDisengageFight);
    }

    public void PlayAnimateMaterial()
    {
        StartCoroutine (AnimateUIMaterial(effectTime));
    }

}
