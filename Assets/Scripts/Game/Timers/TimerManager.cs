using UnityEngine;
using DG.Tweening;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_shortTimeSource;
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private DOTweenAnimation m_dotAnim;
    [SerializeField] private float m_timer = 120f;

    private void Start() => InvokeRepeating("UpdateTimer", 0, 1);

    private void UpdateTimer()
    {
        if (m_timer > 0 && GameManager.instance.IsPlaying)
        {
            m_dotAnim.DORestart();
            m_timer--;
            m_timerText.text = m_timer.ToString();

            ShortTime();
        }
        else if(m_timer <= 0)
            GameManager.instance.EndGame();
    }

    private void ShortTime()
    {
        if(m_timer < 15)
        {
            m_timerText.color = Color.red;
            m_shortTimeSource.Play();
        }
    }
}
