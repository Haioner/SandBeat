using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private StarOrder m_starScorePrefab;

    private float HighScore;
    private float Score;

    private void Awake() => instance = this;
   
    private void Start() => LoadHighscore();

    public void AddScore(int score) { Score += score; }
    public float GetScore() { return Score; }
    public void RestartScore() { Score = 0; }

    public float GetHighScore()
    {
        SaveHighscore();
        LoadHighscore();
        return HighScore;
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

    public void SpawnStars(int score, Transform position)
    {
        AddScore(score);
        StarOrder currentStarOrder = Instantiate(m_starScorePrefab, position.position, Quaternion.identity);
        currentStarOrder.SetStars(score);
    }
}
