using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float m_minSoundCooldown = 0.1f;

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
        float soundSpeed = Mathf.Lerp(0.9f, 1.2f, curveValue); // Ajuste os valores mínimo e máximo de pitch do som aqui
        float minSoundSpeed = 0.5f; // Valor mínimo de pitch do som para garantir que seja audível

        float adjustedSoundSpeed = Mathf.Lerp(minSoundSpeed, 1f, soundSpeed);
        m_scoreSource.pitch = adjustedSoundSpeed;

        float maxWaitTime = Mathf.Lerp(1f, 0.05f, (curveValue - 5f) / 95f); // Ajuste os valores mínimo e máximo de tempo máximo de espera aqui
        float waitTime = Mathf.Lerp(0.1f, maxWaitTime, curveValue / 100f); // Tempo de espera baseado na velocidade da curva

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
        m_highscoreScoreTMP.text = "Highscore " + ScoreManager.instance.GetHighScore().ToString("F0");
    }

    public void BackToMenu()
    {
        ScoreManager.instance.RestartScore();
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        ScoreManager.instance.RestartScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
