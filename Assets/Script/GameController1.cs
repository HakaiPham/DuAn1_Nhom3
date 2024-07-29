using Cinemachine.Utility;
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
    public GameObject _ItemHp;
    public GameObject _ItemMp;
    public GameObject _PanelIntroBoss;
    bool isStartRandom = false;
    bool isStartIntroBoss = false;
    public GameObject _PanelInfomationBoss;
    public GameObject _BossCthuluImage;
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
        GameObject _HpItem = GameObject.FindGameObjectWithTag("ItemHp");
        GameObject _MpItem = GameObject.FindGameObjectWithTag("ItemMp");
        GameObject spawnItemHp;
        GameObject spawnItemMp;
        Debug.Log("So luong quai con: "+_Enemy.Length);
        if(_Enemy.Length <= 0 ) {
            if (isStartIntroBoss == false)
            {
                StartCoroutine(IntroBoss());
                isStartIntroBoss = true;
            }
            monster2Script.enabled = true;
            var randomItemPosition = new Vector2(Random.Range(-7.83f, 13.32f), 0.21f);
            int randomItemSpawn = Random.Range(0, 2);
            if (randomItemSpawn == 0) {
                if (_MpItem != null) return;
                if(_HpItem == null)
                {
                    spawnItemHp = Instantiate(_ItemHp, randomItemPosition, Quaternion.identity);
                    StartCoroutine(SpawnItemCooldown());
                }
            }
            else if (randomItemSpawn == 1)
            {
                if (_HpItem != null) return;
                if(_MpItem == null)
                {
                    spawnItemMp = Instantiate(_ItemMp, randomItemPosition, Quaternion.identity);
                    StartCoroutine(SpawnItemCooldown());
                }
            }
        }
    }
    IEnumerator SpawnItemCooldown()
    {
        yield return new WaitForSeconds(5f);
    }
    IEnumerator IntroBoss()
    {
        _PanelIntroBoss.SetActive(true);
        yield return new WaitForSeconds(2f);
        _PanelIntroBoss.SetActive(false);
        yield return new WaitForSeconds(1f);
        _PanelInfomationBoss.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _BossCthuluImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        _PanelInfomationBoss.SetActive(false);
    }
    IEnumerator DameText(int dame, TextMeshProUGUI text,Transform monsterTransform)
    {
        if (monsterTransform.localScale.x < 0)
        {
            // Quái đang nhìn sang trái, lật DameText theo trục x
            text.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Quái đang nhìn sang phải, giữ DameText hướng bình thường
            text.transform.localScale = new Vector3(1, 1, 1);
        }
        text.gameObject.SetActive(true);
        text.text = dame.ToString("");
        yield return new WaitForSeconds(1.5f);
        text.gameObject.SetActive(false);

    }
    public void StartDameText(int dame,TextMeshProUGUI text, Transform monsterTransform)
    {
        StartCoroutine(DameText(dame, text, monsterTransform));
    }
}
