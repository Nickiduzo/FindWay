using TMPro;
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

    // play default music and set cursor hide, subscribe on listeners
    private void Start()
    {
        AudioManager.instance.Play("Music");

        Cursor.lockState = CursorLockMode.Locked;
    
        Player.WinGame += SwitchWin;
        Player.GameOver += SwitchGameOver;

        Enemy.KillPlayer += SwitchGameOver;
    }
    
    // switch pause if player input 'esc' key and gamewin and gameover are unactive
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameWin.activeInHierarchy && !gameOver.activeInHierarchy)
        {
            SwitchPause();
        }
    }
    public void ContinueButton() => SwitchPause();
    
    public void ExitButton() => Application.Quit();

    // reload game 
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
 
    // if pause is active deactivate pause or activate pause if it's not
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

    // switch on game over ui element, unlock cursor and set time to 0
    private void SwitchGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0f;

        gameOver.gameObject.SetActive(true);
        gameOverTimer.text = TakeFullTime();
    }

    // switch on win ui element, unlock cursor and set time to 0
    private void SwitchWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0f;
        
        gameWin.gameObject.SetActive(true);
        gameWinTimer.text = TakeFullTime();
    }

    // return full time that player spent in game
    private string TakeFullTime()
    {
        float generalTime = timer.time;
        
        int minutes = (int)generalTime / 60;
        
        int seconds = (int)generalTime % 60;
        
        return "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void OnDisable()
    {
        Player.WinGame -= SwitchWin;
        Player.GameOver -= SwitchGameOver;
    }
}
