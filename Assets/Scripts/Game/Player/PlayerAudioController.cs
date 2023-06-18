using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Step")]
    [SerializeField] private AudioSource m_stepSource;
    [SerializeField] private List<AudioClip> m_stepsClips = new List<AudioClip>();
    private int m_currentStep;

    [Header("Dash")]
    [SerializeField] private AudioSource m_dashSource;
    [SerializeField] private List<AudioClip> m_dashClips = new List<AudioClip>();
    private int m_currentDash;

    private MovementController m_controller;
    private Vector3 m_inputMovement;

    private void Awake() => m_controller = GetComponentInParent<MovementController>();
    private void FixedUpdate() => PlayStep();
    private void Update() => PlayDash();

    private void PlayStep()
    {
        if (m_stepSource == null || !GameManager.instance.CanBeat || !m_controller.CanMove) return;

        m_inputMovement.x = Input.GetAxisRaw("Horizontal");
        m_inputMovement.y = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(m_inputMovement.x, m_inputMovement.y).normalized;

        if (move.magnitude > 0 && !m_stepSource.isPlaying)
        {
            NextClip(m_stepSource, m_stepsClips, m_currentStep);
            m_stepSource.Play();
            GameManager.instance.InstantiateSoundVisual(transform);
        }
    }

    private void PlayDash()
    {
        if (m_dashSource == null || !m_controller.m_canDash || !m_controller.CanMove) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NextClip(m_dashSource, m_dashClips , m_currentDash);
            m_dashSource.Play();
            GameManager.instance.InstantiateSoundVisual(transform);
        }
    }

    private void NextClip(AudioSource source, List<AudioClip> clips, int index)
    {
        if (index < clips.Count - 1)
            index++;
        else
            index = 0;

        source.clip = clips[index];
    }
}
