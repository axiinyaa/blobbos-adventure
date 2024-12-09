using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Cannon : MonoBehaviour
{
    public Animator _animation;
    PlayerController player;
    public Transform CannonTarget;
    public UnityEvent onCannonEnter;
    public UnityEvent onFinishShoot;
    public int jumpStrength = 120;
    public int jumpTime = 8;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animation.SetTrigger("ENTERCANNON");

            player = other.gameObject.GetComponent<PlayerCollider>().player;
            player.transform.position = transform.position;

            player.model.SetActive(false);
            player.disableControl = true;

            WaitForShoot();
        }
    }

    async void WaitForShoot()
    {
        await Task.Delay(200);

        onCannonEnter.Invoke();

        await Task.Delay(800);

        _animation.SetTrigger("SHOOT");
    }

    public void Shoot()
    {
        player.model.SetActive(true);
        var tween = player.transform.DOJump(CannonTarget.position, jumpStrength, 1, jumpTime);
        player.animator.SetBool("Shooting", true);
        WaitForTweenEnd(tween);
    }

    async void WaitForTweenEnd(Sequence sequence)
    {
        await sequence.AsyncWaitForCompletion();
        onFinishShoot.Invoke();
        player.animator.SetBool("Shooting", false);
        player.disableControl = false;
    }
}
