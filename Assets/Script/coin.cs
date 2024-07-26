using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class coin : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _CoinText;
    private int coinValue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            coinValue += 10;
            _CoinText.text = coinValue.ToString("");
            Destroy(gameObject); 
        }
    }
}
