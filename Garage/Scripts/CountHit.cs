using TMPro;
using UnityEngine;

public class CountHit : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private int score = 0;

    private void Start()
    {
        scoreText.text = "Score: 0";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            score++;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {score}";
    }
}
