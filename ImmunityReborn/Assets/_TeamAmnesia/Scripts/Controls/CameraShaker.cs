using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }

    [field: SerializeField]
    public CinemachineFreeLook FreeLookCam { get; private set; }

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (shakeTimer > 0.0f)
        {
            shakeTimer -= Time.deltaTime;

            FreeLookCam.GetRig(0)
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0.0f, 1 - shakeTimer / shakeTimerTotal);

            FreeLookCam.GetRig(1)
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0.0f, 1 - shakeTimer / shakeTimerTotal);

            FreeLookCam.GetRig(2)
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain =
                    Mathf.Lerp(startingIntensity, 0.0f, 1 - shakeTimer / shakeTimerTotal);
        }
    }

    public void Shake(float intensity, float time)
    {
        FreeLookCam.GetRig(0)
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;

        FreeLookCam.GetRig(1)
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;

        FreeLookCam.GetRig(2)
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}
