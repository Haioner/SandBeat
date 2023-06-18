using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private CinemachineVirtualCamera m_virtualCamera;
    private float m_shakeTimer;

    private void Awake()
    {
        instance = this;
        m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineChannelPerlin = 
           m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineChannelPerlin.m_AmplitudeGain = intensity;
        m_shakeTimer = time;
    }

    private void Update()
    {
        if(m_shakeTimer > 0)
        {
            m_shakeTimer -= Time.deltaTime;
            if (m_shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineChannelPerlin =
                    m_virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }
}
