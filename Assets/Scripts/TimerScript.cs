using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;

    public PlayerController[] players;

    public GameObject canvas;
    public GameObject text;

    void Start()
    {
        //TimerOn = true;
        canvas.SetActive(false);
        text.SetActive(false);
    }

    void Update()
    {
        players = FindObjectsOfType<PlayerController>();

        if(players.Length == 2 )
        {
            canvas.SetActive(true);
            text.SetActive(true);
            TimerOn = true;
        }
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = "Time Left: " + string.Format("{1:00}", minutes, seconds);
    }

}