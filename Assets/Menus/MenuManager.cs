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
    AudioClip gameMusic;

    private AudioClip curMusic;

    // [SerializeField]
    // Animator titleAnimator;

    private void Start()
    {

        curMusic = menuMusic;
        mainMenuCanvas.SetActive(true);
        //endGameCanvas.SetActive(false);
        Time.timeScale = 0f;

        PlayMenuMusic();

        //titleAnimator.SetTrigger("PlayTitle");



    }

    private void PlayMenuMusic()
    {
        if (audioSource != null)
        {
            if (curMusic != null)
            {
                audioSource.clip = curMusic;
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

    private void ChangeMusic(AudioClip music)
    {
        curMusic = music;

        PlayMenuMusic();
    }

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        ChangeMusic(gameMusic);
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
