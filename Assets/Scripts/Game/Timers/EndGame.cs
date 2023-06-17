using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreTMP;
    [SerializeField] private TextMeshProUGUI m_highscoreScoreTMP;
    [SerializeField] private AnimationCurve m_scoreCurve;
    [SerializeField] private AudioSource m_scoreSource;

    private float m_scoreIncreaseSpeed = 1f;
    private float m_currentScore;
    private bool m_canCountScore;
    private float m_lastSoundTime;

    private void Start()
    {
        LoadScores();
    }

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (m_canCountScore && m_currentScore < ScoreManager.instance.GetScore())
        {
            float progress = m_currentScore / ScoreManager.instance.GetScore();
            float t = m_scoreCurve.Evaluate(progress);

            float scoreIncrease = Time.deltaTime * m_scoreIncreaseSpeed * t;

            m_currentScore += scoreIncrease;
            m_scoreTMP.text = "Score " + m_currentScore.ToString("F0");
            PlayAudio(t);
        }
    }

    private void PlayAudio(float curveValue)
    {
        float soundSpeed = Mathf.Lerp(0.9f, 1.2f, curveValue);
        float minSoundSpeed = 0.5f;

        float adjustedSoundSpeed = Mathf.Lerp(minSoundSpeed, 1f, soundSpeed);
        m_scoreSource.pitch = adjustedSoundSpeed;

        float maxWaitTime = Mathf.Lerp(1f, 0.05f, (curveValue - 5f) / 95f);
        float waitTime = Mathf.Lerp(0.1f, maxWaitTime, curveValue / 100f);

        float timeSinceLastSound = Time.time - m_lastSoundTime;
        if (timeSinceLastSound >= waitTime)
        {
            m_lastSoundTime = Time.time;
            m_scoreSource.Play();
        }
        else if (timeSinceLastSound >= maxWaitTime)
        {
            m_scoreSource.Stop();
        }
    }

    public void LoadScores()
    {
        m_canCountScore = true;
        m_highscoreScoreTMP.text = "<rainb>Highscore " + ScoreManager.instance.GetHighScore().ToString("F0");
    }

    public void BackToMenu()
    {
        ScoreManager.instance.RestartScore();
        FindObjectOfType<Transition>().PlayOutTransition("Menu");
        //SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        ScoreManager.instance.RestartScore();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        string sceneName = SceneManager.GetActiveScene().name;
        FindObjectOfType<Transition>().PlayOutTransition(sceneName);
    }
}
