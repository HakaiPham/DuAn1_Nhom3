using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController1 : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _CoinText;
    int coin;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CollectCoin()
    {
        coin += 10;
        _CoinText.text = coin.ToString("");
    }
}
