using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    public Text text;
    public int minutes;
    public int seconds;
    private float gameTimer = 0f;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;

        minutes = Mathf.FloorToInt(gameTimer / 60);
        seconds = Mathf.FloorToInt(gameTimer % 60);

        text.text = $"{minutes:D2}:{seconds:D2}";
    }
}
