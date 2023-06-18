using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("Beat Timer")]
    [SerializeField] private float m_timeBetween = 0.5f;
    private bool m_canBeat;
    private float m_timer;

    [Header("Audio Source Beat")]
    [SerializeField] private AudioSource m_kickSource;
    [SerializeField] private AudioSource m_snareSource;

    public void PlayButton() => FindObjectOfType<Transition>().PlayOutTransition("Game");

    public void QuitButton() => Application.Quit();

    private void FixedUpdate()
    {
        SetBeat();
        PlayBeat();
    }

    private void SetBeat()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_timeBetween)
        {
            m_canBeat = !m_canBeat;
            m_timer = 0f;
        }
    }

    private void PlayBeat()
    {
        if (m_canBeat && !m_kickSource.isPlaying)
        {
            m_kickSource.Play();
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }

        if (!m_canBeat && !m_snareSource.isPlaying)
        {
            m_snareSource.Play();
            CameraShake.instance.ShakeCamera(1f, 0.05f);
        }
    }
}
