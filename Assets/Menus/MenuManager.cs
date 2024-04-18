using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuCanvas;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip menuMusic;

    [SerializeField]
    Animator titleAnimator;

    private void Start()
    {

        mainMenuCanvas.SetActive(true);
        //endGameCanvas.SetActive(false);
        Time.timeScale = 0f;

        PlayMenuMusic();

        titleAnimator.SetTrigger("PlayTitle");



    }

    private void PlayMenuMusic()
    {
        if (audioSource != null)
        {
            if (menuMusic != null)
            {
                audioSource.clip = menuMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                Debug.Log("No audio clip!");
            }
        }
        else
        {
            Debug.Log("No audiosource!");
        }
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        StopMenuMusic();
        Time.timeScale = 1f;
    }

    private void StopMenuMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
