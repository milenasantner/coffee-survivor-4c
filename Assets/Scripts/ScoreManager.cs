using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    [SerializeField] private TMP_Text scoreText;
    private int score = 0;
    
    void Awake()
    {
        Instance = this;
    }
    
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
        Debug.Log("Score erhöht auf: " + score);
    }
    
    public int GetScore()
    {
        return score;
    }
}