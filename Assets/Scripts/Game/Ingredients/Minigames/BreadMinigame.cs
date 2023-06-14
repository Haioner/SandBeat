using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BreadMinigame : BaseMinigame
{
    [Header("Hands")]
    [SerializeField] private Animator[] m_handsAnim;
    private int m_currentHand;

    [Header("Notes")]
    [SerializeField] private GameObject m_notePrefab;
    [SerializeField] private Transform m_hitsPos;
    [SerializeField] private Transform m_hitter;
    private List<GameObject> m_noteList = new List<GameObject>();

    private float[] m_audioData = new float[512];
    private float m_threshold = 0.1f;
    private bool m_isPlaying = false;
    private bool m_lastAudioPeak = false;

    private void Start() => SpawnNote();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CheckNoteHit();

        if (Input.GetKeyDown(KeyCode.Escape))
            QuitMinigame();

        HandBeater();
    }

    private void QuitMinigame()
    {
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.DestroyInteractCanvas();
        Destroy(gameObject);
    }

    private void SpawnNote()
    {
        GameObject note = Instantiate(m_notePrefab, m_hitsPos);
        m_noteList.Add(note);

        StartCoroutine(DestroyNoteAfterDelay(note, 2.3f));
    }

    private IEnumerator DestroyNoteAfterDelay(GameObject note, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (m_noteList.Contains(note))
        {
            MissAudioSource.Play();
            SpawnNote();
            m_noteList.Remove(note);
            Destroy(note);
        }
    }

    private void CheckNoteHit()
    {
        if (m_noteList.Count == 0)
            return;

        GameObject currentNote = m_noteList[0];

        float distance = Vector3.Distance(currentNote.transform.position, m_hitter.position);
        if (distance >= 0.001f && distance <= 1f)
        {
            PlayRandomClip();
        }
        else
        {
            SpawnNote();
            MissAudioSource.Play();
        }

        Destroy(currentNote);
        m_noteList.RemoveAt(0);
    }

    private void PlayRandomClip()
    {
        int randClip = Random.Range(0, AudioClips.Count);
        MinigameAudioSource.clip = AudioClips[randClip];
        MinigameAudioSource.Play();
        m_isPlaying = true;
    }

    private void HandBeater()
    {
        if (!GameManager.instance.CanBeat) return;

        if (m_isPlaying)
        {
            MinigameAudioSource.GetSpectrumData(m_audioData, 0, FFTWindow.Rectangular);

            bool hasAudioPeak = false;
            for (int i = 0; i < m_audioData.Length; i++)
            {
                if (m_audioData[i] > m_threshold)
                {
                    hasAudioPeak = true;
                    break;
                }
            }

            if (hasAudioPeak && !m_lastAudioPeak)
                AnimateHand();

            m_lastAudioPeak = hasAudioPeak;

            if (!MinigameAudioSource.isPlaying)
                EndIngredient();
        }
    }

    private void EndIngredient()
    {
        m_isPlaying = false;
        GameManager.instance.playerHand.DropItem();
        GameManager.instance.playerHand.SpawnIngredient(Ingredient);
        GameManager.instance.PlayerMovement.SetCanMove(true);
        ingredientManager.DestroyInteractCanvas();
        Destroy(gameObject);
    }

    private void AnimateHand()
    {
        if (m_currentHand < m_handsAnim.Length - 1)
            m_currentHand++;
        else
            m_currentHand = 0;

        m_handsAnim[m_currentHand].SetTrigger("Hit");
    }
}
