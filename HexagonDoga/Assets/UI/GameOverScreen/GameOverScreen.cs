using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = "Score: " + ScoringSystem.Instance.GetScore();
    }

    private void OnEnable()
    {
        tryAgainButton.onClick.AddListener(TryAgain);
    }

    private void OnDisable()
    {
        tryAgainButton.onClick.RemoveListener(TryAgain);
    }

    private void TryAgain()
    {
        GameManager.Instance.GameRestart();
        Destroy(gameObject);
    }
}
