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
    public UnityEvent onFinishShoot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Meow");
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
        await Task.Delay(1000);

        _animation.SetTrigger("SHOOT");
    }

    public void Shoot()
    {
        player.model.SetActive(true);
        var tween = player.transform.DOJump(CannonTarget.position, 120, 1, 8);
        player.animator.SetBool("Shooting", true);
        WaitForTweenEnd(tween);
    }

    async void WaitForTweenEnd(Sequence sequence)
    {
        await sequence.AsyncWaitForCompletion();
        player.transform.position = CannonTarget.position;
        onFinishShoot.Invoke();
        Debug.Log("Awesome");
    }
}
