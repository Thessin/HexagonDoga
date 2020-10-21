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
        GridCreator.Instance.OnActionMade.AddListener(CountDown);
    }

    private void OnDisable()
    {
        GridCreator.Instance.OnActionMade.RemoveListener(CountDown);
    }

    private void CountDown()
    {
        count--;
        countdownText.text = count.ToString();

        if (count == 0)
            GameManager.Instance.GameFinished();
    }
}
