using System;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public static Action<bool,string> OpenDoor;

    [SerializeField] private Animator anim;
    [SerializeField] private string nameOfAnimator;

    private bool isLocked = true;

    private void Update()
    {
        if(anim.GetBool("isOpenDoor")) isLocked = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            OpenDoor?.Invoke(isLocked, nameOfAnimator);
        }
    }
}
