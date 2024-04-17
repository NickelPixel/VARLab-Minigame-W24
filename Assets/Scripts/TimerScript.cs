using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;

    public PlayerController[] players;

    public GameObject canvas;
    public GameObject text;

    public GameObject endScreenUI;
    public PlayerInputManager playerInputManager;

    public GameObject banditWinner;
    public GameObject banditLoser;
    public GameObject sheriffWinner;
    public GameObject sheriffLoser;

    public ScoreDisplay sheriffHorse;
    public ScoreDisplay banditHorse;

    public GameObject fader;

    public GameObject mainCam;


    void Start()
    {
        fader.SetActive(false);
        banditWinner.SetActive(false);
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
                //endScreenUI.SetActive(false);
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);
            }
            else
            {
                fader.SetActive(true);
                
                StartCoroutine(EndGame());
            }
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);

        mainCam.transform.position = new Vector3(0, 5.25f, 4.25f);

        players[0].gameObject.SetActive(false);
        players[1].gameObject.SetActive(false);
        if (banditHorse.totalScore > sheriffHorse.totalScore)
        {
            banditWinner.SetActive(true);
            sheriffLoser.SetActive(true);
        }
        if (sheriffHorse.totalScore > banditHorse.totalScore)
        {
            banditLoser.SetActive(true);
            sheriffWinner.SetActive(true);
        }
        playerInputManager.enabled = false;
        //endScreenUI.SetActive(true);
        Debug.Log("Time is UP!");
        TimeLeft = 0;
        TimerOn = false;
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = "Time Left: " + string.Format("{1:00}", minutes, seconds);
    }

}