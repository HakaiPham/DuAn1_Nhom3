using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    GameController1 gameController1;
    private void Start()
    {
        gameController1 = FindObjectOfType<GameController1>();
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    } 

    public void Home()
    {
        SceneManager.LoadScene("Login");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        gameController1.ResetCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }
  
}
