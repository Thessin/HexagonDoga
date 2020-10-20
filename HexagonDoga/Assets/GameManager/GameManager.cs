using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject gameOverGO;
    [SerializeField] private Transform mainCanvasTrans;

    /// <summary>
    /// Things to do after a GameRestart call is made.
    /// </summary>
    public void GameRestart()
    {

    }

    /// <summary>
    /// Things to do after a GameFinished call is made.
    /// </summary>
    public void GameFinished()
    {
        Instantiate(gameOverGO, mainCanvasTrans);
    }
}
