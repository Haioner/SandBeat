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
    [SerializeField] private GameObject m_hitParticle;
    private List<GameObject> m_noteList = new List<GameObject>();

    private void Start() => SpawnNote();

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            CheckNoteHit();
    }

    private void SpawnNote()
    {
        GameObject note = Instantiate(m_notePrefab, m_hitsPos);
        m_noteList.Add(note);

        StartCoroutine(DestroyNoteAfterDelay(note, 1.35f));
    }

    private IEnumerator DestroyNoteAfterDelay(GameObject note, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (m_noteList.Contains(note))
        {
            GameManager.instance.AddAudioSourcers(MissAudioSource.clip, note.transform);
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
            PlayRandomClip(null);
            InvokeRepeating("AnimateHand", 0, 0.2f);
        }
        else
        {
            SpawnNote();
            MissAudioSource.Play();
        }

        Destroy(currentNote);
        m_noteList.RemoveAt(0);
    }

    public override void PlayRandomClip(Transform audioNoteTransform)
    {
        base.PlayRandomClip(audioNoteTransform);
        Instantiate(m_hitParticle, m_hitter);
        MinigameAudioSource.Play();
    }

    private void AnimateHand()
    {
        if (m_currentHand < m_handsAnim.Length - 1)
            m_currentHand++;
        else
            m_currentHand = 0;

        m_handsAnim[m_currentHand].SetTrigger("Hit");

        if (!MinigameAudioSource.isPlaying)
            EndIngredient();
    }
}
