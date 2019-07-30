using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public float shakeDuration, shakeAmplitude, shakeFrequency;

    private float shakeElapseTime;

    public CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;


    void Awake()
    {
        virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    
    void Update()
    {
        if(shakeElapseTime > 0)
        {
            virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            virtualCameraNoise.m_FrequencyGain = shakeFrequency;
            shakeElapseTime -= Time.deltaTime;
        }
        else
        {
            virtualCameraNoise.m_AmplitudeGain = 0;
            shakeElapseTime = 0;
        }
    }

    public void ShakeItLow()
    {
        shakeAmplitude = .3f;
        shakeElapseTime = shakeDuration;
    }

    public void ShakeItMedium()
    {
        shakeAmplitude = .6f;
        shakeElapseTime = shakeDuration;
    }
}
