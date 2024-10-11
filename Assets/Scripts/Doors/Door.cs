using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Action<bool,string> OpenDoor;

    [SerializeField] private Animator anim;
    [SerializeField] private string nameOfAnimator;

    private bool isLocked = true;
    // unlock door
    private void Update()
    {
        if(anim.GetBool("isOpenDoor")) isLocked = false;
    }

    // check if player stay in collider or contact with collider front of door
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            OpenDoor?.Invoke(isLocked, nameOfAnimator);
    }
}
