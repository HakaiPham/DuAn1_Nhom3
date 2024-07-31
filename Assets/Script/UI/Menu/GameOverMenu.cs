using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    
    Player _Player;

    private void Start()
    {
        _Player = FindObjectOfType<Player>();
    }
    void Update()
    {
        // Kiểm tra xem người chơi còn tồn tại không
        if ( _Player.hpValue <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void GotoHome()
    {       
        SceneManager.LoadScene("Login");
        Time.timeScale = 1;
    }
    public void Restarts()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      

    }
}
