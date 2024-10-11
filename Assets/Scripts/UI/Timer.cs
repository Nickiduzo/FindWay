using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;

    [HideInInspector] public float time;

    private void Start() => time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        TimerUpdate();
    }

    // timer that represent minutes and second spend in game
    private void TimerUpdate()
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        timer.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }
}
