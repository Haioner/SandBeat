using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TomatoMinigame : BaseMinigame
{
    [Header("Position")]
    [SerializeField] private List<RectTransform> m_TomatoList = new List<RectTransform>();

    private void Start()
    {
        StartPositions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            QuitMinigame();
    }

    private void QuitMinigame()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }

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

            // Randomizar a rotação entre 0 e 360
            float randomRotation = Random.Range(0f, 360f);
            tomato.localRotation = Quaternion.Euler(0f, 0f, randomRotation);
        }
    }

    public void SelectTomato(bool isCorrect)
    {
        if (isCorrect)
        {
            PlayRandomClip();
            EndIngredient();
        }
        else
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, transform);
            StartPositions();
        }
    }

    private void PlayRandomClip()
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        GameManager.instance.AddAudioSourcers(AudioClips[0], transform);
    }

    private void EndIngredient()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        GameManager.instance.playerHand.SpawnIngredient(Ingredient);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }
}
