using UnityEngine;
using TMPro;

public class Flag : MonoBehaviour
{
    public GameObject winUI;
    public TextMeshProUGUI scoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "Player" )
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.StopTimer();

            int finalScore = player.GetScore();
            scoreText.text = "Final Score: " + finalScore;

            Time.timeScale = 0;
            winUI.SetActive(true);
        }
    }
}
