using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        animator.Play("FadeOut");
    }

    public void StartFadeIn()
    {
        animator.Play("FadeIn");
    }

    public void NextLevel()
    {
        AsyncNextLevel();
    }

    async void AsyncNextLevel()
    {
        StartFadeIn();
        await Task.Delay(3000);
        SceneManager.LoadScene(1);
    }
}
