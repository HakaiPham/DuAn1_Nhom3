using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController1 : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _CoinText;
    int coin;
    public Monster2 monster2Script;
    void Start()
    {
        monster2Script.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMonsterDestroy();
    }
    public void CollectCoin()
    {
        coin += 10;
        _CoinText.text = coin.ToString("");
    }
    public void CheckMonsterDestroy()
    {
        GameObject[] _Enemy = GameObject.FindGameObjectsWithTag("Monster");
        Debug.Log("So luong quai con: "+_Enemy.Length);
        if(_Enemy.Length <= 0 ) {
            monster2Script.enabled = true;
        }
    }
}
