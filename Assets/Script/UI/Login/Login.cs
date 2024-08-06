using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import SceneManager

public class Login : MonoBehaviour
{
    // Thay đổi cảnh khi bấm nút
    [SerializeField] GameObject SelectLVMenu;
    [SerializeField] GameObject WaitUpdateMenu;
    [SerializeField] GameObject BXHMenu;
    [SerializeField] GameObject HelpMenu;
    [SerializeField] GameObject DemoE;
    [SerializeField] GameObject DemoF;
    [SerializeField] GameObject DemoR;
    [SerializeField] GameObject DemoArrow;
    [SerializeField] GameObject DemoSpace;
    [SerializeField] GameObject Demo1;
    [SerializeField] GameObject Demo2;
    public void ChangeScene()
    {
        // Tên cảnh cần chuyển đến
        SceneManager.LoadScene("Scene2");
    }
    public void Help()
    {
        HelpMenu.SetActive(true);
        Time.timeScale = 1;

    }
    public void HelpExit()
    {
        HelpMenu.SetActive(false);
        Time.timeScale = 1;

    }
    public void DemoSkillE()
    {
        DemoE.SetActive(true);
        Time.timeScale = 1;

    }
    public void DemoSkillEExit()
    {
        DemoE.SetActive(false);
        Time.timeScale = 1;

    }
    public void DemoSkillF()
    {
        DemoF.SetActive(true);
        Time.timeScale = 1;
    }
    public void DemoSkillFExit()
    {
        DemoF.SetActive(false);
        Time.timeScale = 1;
    }
    public void DemoSkillR()
    {
        DemoR.SetActive(true);
        Time.timeScale = 1;
       
    }
    public void DemoSkillRExit()
    {
        DemoR.SetActive(false);
        Time.timeScale = 1;

    }
    public void DemoSkillArrow()
    {
        DemoArrow.SetActive(true);
        Time.timeScale = 1;
    }
    public void DemoSkillArrowExit()
    {
        DemoArrow.SetActive(false);
        Time.timeScale = 1;
    }
    public void DemoSkillSpace()
    {
        DemoSpace.SetActive(true);
        Time.timeScale = 1;
    }
    public void DemoSkillSpaceExit()
    {
        DemoSpace.SetActive(false);
        Time.timeScale = 1;
    }
    public void DemoSkill1()
    {
        Demo1.SetActive(true);
        Time.timeScale = 1;
    }
    public void DemoSkill1Exit()
    {
        Demo1.SetActive(false);
        Time.timeScale = 1;
    }
    public void DemoSkill2()
    {
        Demo2.SetActive(true);
        Time.timeScale = 1;
    }
    public void DemoSkill2Exit()
    {
        Demo2.SetActive(false);
        Time.timeScale = 1;
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
        Time.timeScale = 1;
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

    public void ExitGame()
    {
        Application.Quit();
    }
}
