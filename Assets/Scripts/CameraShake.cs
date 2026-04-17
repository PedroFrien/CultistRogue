using Unity.Cinemachine;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    private CinemachineCamera cmCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    private float shakeTimer;

    private void Awake()
    {
        cmCamera = GetComponent<CinemachineCamera>();
        perlin = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {

        perlin.AmplitudeGain = intensity;

        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer < 0)
            {
                perlin.AmplitudeGain = 0f;
            }
        }
    }
}
