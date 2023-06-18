using System.Collections.Generic;
using UnityEngine;

public class TomatoMinigame : BaseMinigame
{
    [Header("Position")]
    [SerializeField] private List<RectTransform> m_TomatoList = new List<RectTransform>();

    private void Start() => StartPositions();

    private void StartPositions()
    {
        foreach (var tomato in m_TomatoList)
        {
            float randPosX = Random.Range(-128f, 128f);
            float randPosY = Random.Range(-54f, 54f);
            Vector3 newPos = tomato.anchoredPosition;
            newPos.x = randPosX;
            newPos.y = randPosY;
            tomato.anchoredPosition = newPos;

            //Randomize rotation
            float randomRotation = Random.Range(0f, 360f);
            tomato.localRotation = Quaternion.Euler(0f, 0f, randomRotation);
        }
    }

    public void SelectTomato(bool isCorrect)
    {
        if (isCorrect)
        {
            PlayRandomClip(transform);
            EndIngredient();
        }
        else
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, transform);
            StartPositions();
        }
    }
}
