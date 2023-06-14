using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private VolumeProfile m_profile;
    [SerializeField] private Vector2 m_randomRange;

    private void Start()
    {
        InvokeRepeating("ChangeColor", 0, 1);   
    }

    private void OnApplicationQuit()
    {
        if (m_profile.TryGet(out ColorAdjustments colorAdjustments))
            colorAdjustments.hueShift.value = 0;
    }

    private void ChangeColor()
    {
        if (!GameManager.instance.IsPlaying) return;

        float randNumber = Random.Range(m_randomRange.x, m_randomRange.y);

        if(m_profile.TryGet(out ColorAdjustments colorAdjustments))
            colorAdjustments.hueShift.value = randNumber;
        
    }
}
