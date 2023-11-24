using System;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public static int Score=0;
    public TMPro.TextMeshProUGUI ScoreText;

    private void Update()
    {
        ScoreText.text = "Score: " + Score;
    }
}
