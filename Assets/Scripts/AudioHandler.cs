using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public void PlayOneShotRP(AudioClip sound)
    {
        GetComponent<AudioSource>().pitch = Random.Range(0.5f, 1.5f);
        GetComponent<AudioSource>().PlayOneShot(sound);
    }

    public void PlayOneShot(AudioClip sound)
    {
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}
