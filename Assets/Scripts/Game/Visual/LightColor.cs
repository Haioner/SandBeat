using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    [SerializeField] private Light2D m_light;

    void Start() => InvokeRepeating("RandomLightColor", 0, 1);

    private void RandomLightColor()
    {
        if (!GameManager.instance.IsPlaying) return;

        m_light.color = RandomColor.Generate();
    }
}
