using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [Header("Ingredient")]
    public IngredientSO Ingredient;
    [HideInInspector] public IngredientManager ingredientManager;

    [Header("Audios")]
    public AudioSource MinigameAudioSource;
    public AudioSource MissAudioSource;
    public List<AudioClip> AudioClips = new List<AudioClip>();
}
