using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Audio Manager")]
    public float TimeBetween = 0.5f;
    public bool CanBeat { get; private set; }
    private float m_timer;

    [Header("Audio Source Beat")]
    [SerializeField] private AudioSource m_kickSource;
    [SerializeField] private AudioSource m_snareSource;

    public void PlayButton()
    {
        FindObjectOfType<Transition>().PlayOutTransition("Game");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        SetBeat();
        PlayBeat();
    }

    private void SetBeat()
    {
        m_timer += Time.deltaTime;
        if (m_timer > TimeBetween)
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
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }

        if (!CanBeat && !m_snareSource.isPlaying)
        {
            m_snareSource.Play();
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }
    }
}
