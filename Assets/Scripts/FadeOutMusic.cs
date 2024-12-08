using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutMusic : MonoBehaviour
{
    public AudioSource source;
    public Animator animator;

    public void FadeOut()
    {
        animator.Play("SongFade");
    }
}
