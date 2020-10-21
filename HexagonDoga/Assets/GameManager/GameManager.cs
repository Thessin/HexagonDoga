using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private Transform mainCanvasTrans;

    private bool gameFinished = false;

    /// <summary>
    /// Things to do after a GameRestart call is made.
    /// </summary>
    public void GameRestart()
    {
        gameFinished = false;
        GridCreator.Instance.RecreateGrid();
        ScoringSystem.Instance.ResetScore();
    }

    /// <summary>
    /// Things to do after a GameFinished call is made.
    /// </summary>
    public void GameFinished()
    {
        if (!gameFinished)
        {
            Instantiate(gameOverGO, mainCanvasTrans);
            gameFinished = true;
        }
    }
}
