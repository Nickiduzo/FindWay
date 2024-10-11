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

    // use timer for hit
    private void Update()
    {
        HideHint();
    }
    
    // unlock door if player is nearly and play animation
    private void UnlockDoor(bool isLocked, string doorName)
    {
        if (isLocked && KeyUI.keysAmount > 0)
        {
            Animator anim = FindDoor(doorName);
            anim.SetBool("isOpenDoor", true);
        }
        else if(isLocked)
        {
            timer = 5f;
            needKey.gameObject.SetActive(true);
        }
    }

    // timer for hide hunt
    private void HideHint()
    {
        if(timer <= 0f) needKey.gameObject.SetActive(false);
        else timer -= Time.deltaTime;
    }

    // find door with string name and return
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
