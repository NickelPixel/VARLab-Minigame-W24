using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    PlayerController[] feeder;

    RoastingReticle feederReticle;

    [SerializeField]
    GameObject scorePanel;

    [SerializeField]
    TextMeshProUGUI horseInfo, scoreDisplay;

    [SerializeField]
    float totalScore = 0f;

    [SerializeField]
    float currentScore = 0f;

    [SerializeField]
    private float healthyThreshold = 50f, sickThreshold = 0f;



    void Start()
    {
        //feeder = FindObjectsOfType<PlayerController>();
        scorePanel.SetActive(true);



        UpdateFeederArray();

    }


    public void OnPlayerJoined()
    {
        UpdateFeederArray();
        Debug.Log("Player joined!");
    }

    public void OnPlayerLeft()
    {
        UpdateFeederArray();
        Debug.Log("Player left!");
    }

    private void UpdateFeederArray()
    {
        feeder = FindObjectsOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (PlayerController currentFeeder in feeder)
        {
            if (currentFeeder.feeding)
            {
                Debug.Log(currentFeeder + " is feeding");
                Score(currentFeeder);

            }

        }


    }


    private RoastingReticle FeederRecticle(PlayerController feeder)
    {
        feederReticle = feeder.GetComponentInChildren<RoastingReticle>(true);
        if (feederReticle != null)
        {
            return feederReticle;

        }
        else
        {
            Debug.Log("Didn't find RoastingReticle on" + feeder.gameObject.name);
            return null;
        }
    }

    private void Score(PlayerController feeder)
    {
        RoastingReticle currentRecticle = FeederRecticle(feeder);
        if (currentRecticle != null)
        {
            if (!currentRecticle.scoreCalculated)
            {
                currentRecticle.scoreCalculated = true;

                currentScore = currentRecticle.finalScore;

                if (currentScore < 0)
                {
                    horseInfo.text = "Your Horse Is Getting Hurt!";
                }
                else
                {
                    horseInfo.text = "Your Horse Is Getting Feed!";

                }

                Debug.Log("Getting feed! The current score is: " + currentScore);
                totalScore += currentScore;

                Invoke("ShowScore", feeder.feedTime);

            }

        }

    }

    private void ShowScore()
    {
        scoreDisplay.text = totalScore.ToString("F2");
        if (totalScore > healthyThreshold)
        {
            horseInfo.text = "Your Horse Is Fantastic!";
        }
        else if (totalScore < sickThreshold)
        {
            horseInfo.text = "Your Horse Is Sick!";

        }
        else
        {
            horseInfo.text = "Your Horse Is Healthy!";
        }
    }

    // private IEnumerator CalculateScore(PlayerController feeder)
    // {
    //     if (feeder.feedTime > 0)
    //     {
    //         currentScore = FeederRecticle(feeder).finalScore;
    //         Debug.Log("Getting feed! The current score is: " + currentScore);
    //     }

    //     yield return new WaitForSeconds(feeder.feedTime);
    //     totalScore += currentScore;
    //     scoreDisplay.text = totalScore.ToString("F2");

    // }


}
