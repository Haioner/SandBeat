using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreTMP;
    [SerializeField] private TextMeshProUGUI m_highscoreScoreTMP;
    private float m_currentScore;
    private bool m_canCountScore;

    private void Start()
    {
        LoadScores();
    }

    private void Update()
    {
        if (m_canCountScore && m_currentScore < ScoreManager.instance.GetScore())
        {
            m_currentScore += Time.deltaTime * 10f;
            m_scoreTMP.text = "Score " + m_currentScore.ToString("F0");
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
