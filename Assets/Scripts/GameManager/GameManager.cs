using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // here could be singleton pattern, but let use whitout;

    [SerializeField] private GameObject gameWin;
    [SerializeField] private TextMeshProUGUI gameWinTimer;

    [SerializeField] private GameObject gameOver;
    [SerializeField] private TextMeshProUGUI gameOverTimer;

    [SerializeField] private GameObject pause;

    [SerializeField] private Timer timer;
    private void Start()
    {
        AudioManager.instance.Play("Music");

        Cursor.lockState = CursorLockMode.Locked;
        Player.WinGame += SwitchWin;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchPause();
    }
    public void ContinueButton() => SwitchPause();
    
    public void ExitButton() => Application.Quit();

    public void RestartGame() => SceneManager.LoadScene(0);
 

    private void SwitchPause()
    {
        if(Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            pause.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pause.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    private void SwtichGameOver()
    {
        gameOver.gameObject.SetActive(true);

        float generalTime = timer.time;
        int minutes = (int)generalTime / 60;
        int seconds = (int)generalTime % 60;
        gameOverTimer.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void SwitchWin()
    {
        gameWin.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        Player.WinGame -= SwitchWin;
    }
}
