using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject CheckPlayer;

    void Update()
    {
        // Kiểm tra xem người chơi còn tồn tại không
        if (CheckPlayer == null)
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
        Time.timeScale = 1;
        SceneManager.LoadScene("Login");
    }
}
