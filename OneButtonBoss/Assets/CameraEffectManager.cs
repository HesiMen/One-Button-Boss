using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraEffectManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noiseComponent;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float amplitude, frequency;

    // Update is called once per frame
    void Awake()
    {
        noiseComponent = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }
    }

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCamera(shakeDuration, amplitude, frequency));
    }

    IEnumerator ShakeCamera(float shakeDuration, float amplitude = 1, float frequency = 1)
    {
        noiseComponent.m_AmplitudeGain = amplitude;
        noiseComponent.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(shakeDuration);
        noiseComponent.m_AmplitudeGain = 0;
        noiseComponent.m_FrequencyGain = 0;
    }
}
