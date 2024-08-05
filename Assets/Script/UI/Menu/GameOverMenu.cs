using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    
    Player _Player;
    bool isDead;
    GameController1 _Controller1;
    private void Start()
    {
        _Player = FindObjectOfType<Player>();
        isDead = false;
        _Controller1 = FindObjectOfType<GameController1>();
    }
    void Update()
    {
        // Kiểm tra xem người chơi còn tồn tại không
        if ( _Player.hpValue <= 0&&isDead==false)
        {
            isDead = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }

    public void GotoHome()
    {       
        SceneManager.LoadScene("Login");
    }
    public void Restarts()
    {
        Time.timeScale = 1;
        _Controller1.ResetCoin();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      
    }
}
