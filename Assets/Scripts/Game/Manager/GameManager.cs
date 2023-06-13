using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool IsPlaying { get; private set; }

    [Header("Audio Manager")]
    public float TimeBetween = 0.5f;
    public bool CanBeat { get; set; }
    private float m_timer;

    [Header("Audio Source Beat")]
    [SerializeField] private AudioSource m_kickSource;
    [SerializeField] private AudioSource m_snareSource;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (!IsPlaying) return;

        SetBeat();
        PlayBeat();
    }

    private void SetBeat()
    {
        m_timer += Time.deltaTime;
        if(m_timer > TimeBetween)
        {
            CanBeat = !CanBeat;
            m_timer = 0f;
        }
    }

    private void PlayBeat()
    {
        if (CanBeat && !m_kickSource.isPlaying)
            m_kickSource.Play();

        if(!CanBeat && !m_snareSource.isPlaying)
            m_snareSource.Play();
    }

    public void SetPlaying() => IsPlaying = true;
}
