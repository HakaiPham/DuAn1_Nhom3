using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GateLoading : MonoBehaviour
{
    [SerializeField] GameObject _loadingCanvat;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] GameObject _ArrowGame;
    //private void Start()
    //{
    //    _loadingCanvat.SetActive(false);
    //}
    private void Update()
    {
        CheckClearMonster();
    }
    public void CheckClearMonster()
    {
        GameObject[] checkMonster = GameObject.FindGameObjectsWithTag("Monster");
        GameObject checkBoss1 = GameObject.FindGameObjectWithTag("EnemySummon");
        var currentScene = SceneManager.GetActiveScene().name;
        if (checkMonster.Length <= 0&& currentScene != "Scene3_5")
        {
            _ArrowGame.SetActive(true);
        }
        if(checkMonster.Length <= 0 && checkBoss1 == null)
        {
            _ArrowGame.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject[] checkMonster = GameObject.FindGameObjectsWithTag("Monster");
            GameObject checkBoss1 = GameObject.FindGameObjectWithTag("EnemySummon");
            // Loading.....
            var currentScene = SceneManager.GetActiveScene().name;
            if ( checkMonster.Length <= 0&& currentScene != "Scene3_5")
            {
                _loadingCanvat.SetActive(true);
                StartCoroutine(Loading());
            }
            else if (checkMonster.Length <= 0 && checkBoss1 == null
                && currentScene == "Scene3_5")
            {
                _loadingCanvat.SetActive(true);
                StartCoroutine(Loading());
                _ArrowGame.SetActive(true);
            }
        }
    }
    IEnumerator Loading()
    {
        var value = 0;
        _slider.value = value;
        while (value < 100)
        {
            value += 10;
            _slider.value = value;
            _textMeshProUGUI.text = value + "%";
            yield return new WaitForSeconds(0.2f); // Chạy mỗi 0.6 giây để tăng 20%
        }
        var currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Scene tiếp theo là: " + currentScene);
        switch (currentScene)
        {
            case "Scene1": SceneManager.LoadScene("Scene2"); break;
            case "Scene2": SceneManager.LoadScene("Scene3_1"); break;
            case "Scene3_1": SceneManager.LoadScene("Scene3_5"); break;
            case "Scene3_5": SceneManager.LoadScene("Scene4"); break;
        }
    }
}
