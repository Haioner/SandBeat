using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private float Score;
    private float HighScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadHighscore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            AddScore(50);
    }

    public float GetScore()
    {
        return Score;
    }

    public float GetHighScore()
    {
        SaveHighscore();
        LoadHighscore();
        return HighScore;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public void RestartScore()
    {
        Score = 0;
    }

    public void SaveHighscore()
    {
        if (!PlayerPrefs.HasKey("highscore"))
            PlayerPrefs.SetFloat("highscore", Score);
        else if (Score > PlayerPrefs.GetFloat("highscore"))
            PlayerPrefs.SetFloat("highscore", Score);
    }

    private void LoadHighscore()
    {
        if (PlayerPrefs.HasKey("highscore"))
            HighScore = PlayerPrefs.GetFloat("highscore");
    }
}
