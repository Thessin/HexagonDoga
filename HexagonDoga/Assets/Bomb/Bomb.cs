using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private int count = 5;

    private void Start()
    {
        countdownText.text = count.ToString();
    }

    private void OnEnable()
    {
        GridSystem.Instance.OnActionMade.AddListener(CountDown);
    }

    private void OnDisable()
    {
        GridSystem.Instance.OnActionMade.RemoveListener(CountDown);
    }

    private void CountDown()
    {
        count--;
        countdownText.text = count.ToString();

        if (count == 0)
            GameManager.Instance.GameFinished();
    }
}
