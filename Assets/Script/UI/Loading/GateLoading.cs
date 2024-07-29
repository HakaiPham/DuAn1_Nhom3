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

    //private void Start()
    //{
    //    _loadingCanvat.SetActive(false);
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Loading.....

            _loadingCanvat.SetActive(true);
            StartCoroutine(Loading());
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
            yield return new WaitForSeconds(0.1f); // Chạy mỗi 0.6 giây để tăng 20%
        }
        // chuyển sang sc2
        SceneManager.LoadScene(3);
    }
}
