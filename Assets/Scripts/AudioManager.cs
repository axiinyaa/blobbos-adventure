using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource source;

    public void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
