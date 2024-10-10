using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private List<Animator> doorAnimators = new List<Animator>();
    [SerializeField] private KeyUI KeyUI;
    [SerializeField] private TextMeshProUGUI needKey;

    private float timer;
    private void Start()
    {
        timer = 0f;
        Door.OpenDoor += UnlockDoor;
    }

    private void Update()
    {
        HideHint();
    }
    private void UnlockDoor(bool isLocked, string doorName)
    {
        if (isLocked && KeyUI.keysAmount > 0)
        {
            Animator anim = FindDoor(doorName);
            anim.SetBool("isOpenDoor", true);
        }
        else
        {
            timer = 5f;
            needKey.gameObject.SetActive(true);
        }
    }

    private void HideHint()
    {
        if(timer <= 0f) needKey.gameObject.SetActive(false);
        else timer -= Time.deltaTime;
    }
    private Animator FindDoor(string name)
    {
        Animator animator = null;
        
        foreach(var anim in doorAnimators)
            if (anim.name == name)
            {
                animator = anim;
                KeyUI.keysAmount--;
            }
       
        return animator;
    }
    
    private void OnDisable()
    {
        Door.OpenDoor -= UnlockDoor;
    }
}
