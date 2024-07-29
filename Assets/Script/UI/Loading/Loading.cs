using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] GameObject _LoginCanvas;
    [SerializeField] GameObject _loadingCanvas;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _textMeshProUGUI;

    //private void Start()
    //{
    //    _loadingCanvas.SetActive(false);
    //}

    public void LoadingScenes()
    {
        _LoginCanvas.SetActive(false);
        _loadingCanvas.SetActive(true);
        StartCoroutine(LoadingS());
    }

    IEnumerator LoadingS()
    {
        var value = 0;
        _slider.value = value;
        while (value < 100)
        {
            value += 10;
            _slider.value = value;
            _textMeshProUGUI.text = value + "%";
            yield return new WaitForSeconds(0.1f); // Chạy mỗi 0.6 giây để tăng 20%
        }
        // chuyển sang sc2
        SceneManager.LoadScene(1);
    }
}
