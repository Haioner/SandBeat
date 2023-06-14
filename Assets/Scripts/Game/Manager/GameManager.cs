using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool IsPlaying { get; private set; }

    [Header("Player")]
    public MovementController PlayerMovement;
    public PlayerHand playerHand;

    [Header("Audio Manager")]
    public float TimeBetween = 0.5f;
    public bool CanBeat { get; private set; }
    private float m_timer;

    [Header("Audio Source Beat")]
    [SerializeField] private AudioSource m_kickSource;
    [SerializeField] private AudioSource m_snareSource;
    [SerializeField] private GameObject m_soundEffect;
    public UnityEvent AudioSources;

    [Header("Minigames")]
    public float MinigameSpeed = 7f;

    private void Awake() => instance = this;

    private void FixedUpdate()
    {
        if (!IsPlaying) return;

        SetBeat();
        PlayBeat();
    }

    public void SetPlaying() => IsPlaying = true;

    public void EndGame()
    {
        IsPlaying = false;
        PlayerMovement.SetCanMove(false);
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
        {
            m_kickSource.Play();
            AudioSources?.Invoke();
            AudioSources?.RemoveAllListeners();
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }

        if(!CanBeat && !m_snareSource.isPlaying)
        {
            m_snareSource.Play();
            AudioSources?.Invoke();
            AudioSources?.RemoveAllListeners();
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }
    }

    public void InstantiateSoundVisual(Transform soundPos)
    {
        Instantiate(m_soundEffect, soundPos.position, Quaternion.identity);

    }

    public void AddAudioSourcers(AudioSource source, Transform soundPos)
    {
        if (!IsAudioSourceInList(source))
        {
            AudioSources.AddListener(() => source.Play());
            InstantiateSoundVisual(soundPos);
        }
    }

    private bool IsAudioSourceInList(AudioSource source)
    {
        for (int i = 0; i < AudioSources.GetPersistentEventCount(); i++)
        {
            if (AudioSources.GetPersistentTarget(i) is AudioSource audioSource && audioSource == source)
            {
                return true;
            }
        }

        return false;
    }
}
