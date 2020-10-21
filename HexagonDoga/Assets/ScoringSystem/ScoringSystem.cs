using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoringSystem : Singleton<ScoringSystem>
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore = 0;
    private int bombPerXScore = 25;       // Attach bomb per x score.

    private void Start()
    {
        UpdateScoreText();
    }

    /// <summary>
    /// Add score to scoreboard.
    /// </summary>
    /// <param name="points"></param>
    public void AddScore(int points)
    {
        currentScore += points;

        if (currentScore % bombPerXScore == 0)
            GridCreator.Instance.AttachBombToRandom();

        UpdateScoreText();
    }

    /// <summary>
    /// Update the scoreText.
    /// </summary>
    private void UpdateScoreText()
    {
        scoreText.text = "Score: "+currentScore.ToString();
    }

    /// <summary>
    /// Returns current score.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Resets score.
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }
}
