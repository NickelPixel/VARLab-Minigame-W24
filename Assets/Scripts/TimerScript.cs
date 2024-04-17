using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;

    public TextMeshProUGUI TimerTxt;
    public TextMeshProUGUI sheriffPoints;
    public TextMeshProUGUI banditPoints;
    public GameObject sheriffBG;
    public GameObject banditBG;
    public GameObject playAgain;

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

    public bool gameOver;


    void Start()
    {
        gameOver = false;
        playAgain.SetActive(false);
        sheriffBG.SetActive(false);
        banditBG.SetActive(false);
        banditPoints.enabled = false;
        sheriffPoints.enabled = false;
        fader.SetActive(false);
        banditWinner.SetActive(false);
        //TimerOn = true;
        canvas.SetActive(false);
        text.SetActive(false);
    }

    

    public void OnEnd(InputAction.CallbackContext context)
    {
        if (gameOver)
        {
            SceneManager.LoadScene(1);
        }
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

        gameOver = true;

        playAgain.SetActive(true);
        mainCam.transform.position = new Vector3(0, 5.25f, 4.25f);
        
        players[0].gameObject.SetActive(false);
        players[1].gameObject.SetActive(false);
        if (banditHorse.totalScore > sheriffHorse.totalScore)
        {
            
            TimerTxt.text = "Bandit Wins!";
            banditWinner.SetActive(true);
            sheriffLoser.SetActive(true);
        }
        if (sheriffHorse.totalScore > banditHorse.totalScore)
        {
            TimerTxt.text = "Sheriff Wins!";
            banditLoser.SetActive(true);
            sheriffWinner.SetActive(true);
        }
        playerInputManager.enabled = false;
        //endScreenUI.SetActive(true);
        Debug.Log("Time is UP!");
        TimeLeft = 0;
        TimerOn = false;
        sheriffBG.SetActive(true);
        banditBG.SetActive(true);
        banditPoints.enabled = true;
        sheriffPoints.enabled = true;
        sheriffPoints.text = sheriffHorse.totalScore.ToString("F1");
        banditPoints.text = banditHorse.totalScore.ToString("F1");
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = "Time Left: " + string.Format("{1:00}", minutes, seconds);
    }

}