using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Login : MonoBehaviour
{
    // Thay đổi cảnh khi bấm nút
    [SerializeField] GameObject SelectLVMenu;
    [SerializeField] GameObject WaitUpdateMenu;
    [SerializeField] GameObject BXHMenu;
    public void ChangeScene()
    {
        // Tên cảnh cần chuyển đến
        SceneManager.LoadScene("Scene2");
    }

    public void SelectLV()
    {
       SelectLVMenu.SetActive(true);
        Time.timeScale = 1;
    }

    public void SelectLVExit()
    {
        SelectLVMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void WaitUpdate()
    {
        WaitUpdateMenu.SetActive(true);
        Time .timeScale = 1;
    }
    public void WaitUpdateEXit()
    {
        WaitUpdateMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void BXH()
    {
        BXHMenu.SetActive(true);
        Time.timeScale = 1;
    }
    public void BXHExit()
    {
        BXHMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Scene1()
    {
        SceneManager.LoadScene("Scene1");
        Time.timeScale = 1;
    }

    public void Scene2()
    {
        SceneManager.LoadScene("Scene2");
        Time.timeScale = 1;
    }
    public void Scene3()
    {
        SceneManager.LoadScene("Scene3_1");
        Time.timeScale = 1;
    }
    public void Scene4()
    {
        SceneManager.LoadScene("Scene3_5");
        Time.timeScale = 1;
    }
    public void Scene5()
    {
        SceneManager.LoadScene("Scene4");
        Time.timeScale = 1;
    }
}
