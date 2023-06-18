using System.Collections.Generic;
using UnityEngine;

public class BaseMinigame : MonoBehaviour
{
    [Header("Ingredient")]
    public IngredientSO Ingredient;
    [HideInInspector] public IngredientManager ingredientManager;

    [Header("Audios")]
    public AudioSource MinigameAudioSource;
    public List<AudioClip> AudioClips = new List<AudioClip>();
    public AudioSource MissAudioSource;

    public virtual void Update() => QuitMinigame();

    public void QuitMinigame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            GameManager.instance.PlayerMovement.SetCanMove(true);
            ingredientManager.SetCameraTarget(0);
            Destroy(gameObject);
        }
    }

    public virtual void EndIngredient()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        GameManager.instance.playerHand.SpawnIngredient(Ingredient);
        ingredientManager.SetCameraTarget(0);
        Destroy(gameObject);
    }

    public virtual void PlayRandomClip(Transform audioNoteTransform)
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        if (audioNoteTransform != null)
            GameManager.instance.AddAudioSourcers(MinigameAudioSource.clip, audioNoteTransform);
    }
}
