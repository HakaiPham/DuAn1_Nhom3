using Cinemachine.Utility;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController1 : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _CoinText;
    public static int coin = 0;
    public Monster2 monster2Script;
    public GameObject _ItemHp;
    public GameObject _ItemMp;
    public GameObject _PanelIntroBoss;
    bool isStartRandom = false;
    bool isStartIntroBoss = false;
    public GameObject _PanelInfomationBoss;
    public GameObject _BossCthuluImage;
    public GameObject _PanelInfomationBoss2;
    public GameObject _BossDarkWizardImage;
    public GameObject _PanelTotalGame;
    public GameObject _BossWizard;
    bool isTotalGameOn = false;
    string currentScene;
    public TextMeshProUGUI _ToTalCoinText;
    public TextMeshProUGUI _NpcTalk1;
    public TextMeshProUGUI _NpcTalk2;
    public GameObject _PanelNPCTalk;
    bool isNpcTalk = false;
    public GameObject _CoinSpawn;
    void Start()
    {
        _CoinText.text = coin.ToString("");
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMonsterDestroy();
        if(currentScene== "Scene4") {
            if (_BossWizard == null && isTotalGameOn == false)
            {
                isTotalGameOn = true;
                StartCoroutine(TotalGame());
            }
        }
        if(isNpcTalk == false&& Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(NPCTalk());
            isNpcTalk = false;
        }
    }
    public void CollectCoin()
    {
        coin += 10;
        _CoinText.text = coin.ToString("");
    }
    public static int GetScore()
    {
        return coin;
    }
    public void CheckMonsterDestroy()
    {
        GameObject[] _Enemy = GameObject.FindGameObjectsWithTag("Monster");
        GameObject _HpItem = GameObject.FindGameObjectWithTag("ItemHp");
        GameObject _MpItem = GameObject.FindGameObjectWithTag("ItemMp");
        GameObject _EnemySummon = GameObject.FindGameObjectWithTag("EnemySummon");
        GameObject _DarkWizard = GameObject.FindGameObjectWithTag("BossTank");
        GameObject _Coin = GameObject.FindGameObjectWithTag("Coin");
        GameObject spawnItemHp;
        GameObject spawnItemMp;
        if (_EnemySummon == null && _DarkWizard == null) return;
        Debug.Log("So luong quai con: "+_Enemy.Length);
        if(_Enemy.Length <= 0 && _EnemySummon!=null) {
            if (isStartIntroBoss == false)
            {
                StartCoroutine(IntroBoss());
                isStartIntroBoss = true;
            }
            monster2Script.enabled = true;
            var randomItemPosition = new Vector2(Random.Range(-7.83f, 13.32f), 0.21f);
            int randomItemSpawn = Random.Range(0, 2);
            if (_Coin==null)
            {
                var spawnCoin = Instantiate(_CoinSpawn, randomItemPosition, Quaternion.identity);
            }
            if (randomItemSpawn == 0) {
                if (_MpItem != null) return;
                if(_HpItem == null)
                {
                    spawnItemHp = Instantiate(_ItemHp, randomItemPosition, Quaternion.identity);
                }
            }
            else if (randomItemSpawn == 1)
            {
                if (_HpItem != null) return;
                if(_MpItem == null)
                {
                    spawnItemMp = Instantiate(_ItemMp, randomItemPosition, Quaternion.identity);
                }
            }
            //if (_Coin != null) return;
        }
        else if(_DarkWizard != null)
        {
            if (isStartIntroBoss == false)
            {
                StartCoroutine(IntroBoss2());
                isStartIntroBoss = true;
            }
            var randomItemPosition = new Vector2(Random.Range(-7.57f, 6.62f), -1.841413f);
            int randomItemSpawn = Random.Range(0, 2);
            if (_Coin == null)
            {
                var spawnCoin = Instantiate(_CoinSpawn, randomItemPosition, Quaternion.identity);
            }
            if (randomItemSpawn == 0)
            {
                if (_MpItem != null) return;
                if (_HpItem == null)
                {
                    spawnItemHp = Instantiate(_ItemHp, randomItemPosition, Quaternion.identity);
                }
            }
            else if (randomItemSpawn == 1)
            {
                if (_HpItem != null) return;
                if (_MpItem == null)
                {
                    spawnItemMp = Instantiate(_ItemMp, randomItemPosition, Quaternion.identity);
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
    IEnumerator IntroBoss2()
    {
        _PanelIntroBoss.SetActive(true);
        yield return new WaitForSeconds(2f);
        _PanelIntroBoss.SetActive(false);
        yield return new WaitForSeconds(1f);
        _PanelInfomationBoss2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _BossDarkWizardImage.SetActive(true);
        yield return new WaitForSeconds(4.2f);
        _PanelInfomationBoss2.SetActive(false);
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
        yield return new WaitForSeconds(0.1f);
        text.gameObject.SetActive(false);

    }
    public void StartDameText(int dame,TextMeshProUGUI text, Transform monsterTransform)
    {
        StartCoroutine(DameText(dame, text, monsterTransform));
    }
    IEnumerator TotalGame()
    {
        Time.timeScale = 0;
        _PanelTotalGame.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        _ToTalCoinText.gameObject.SetActive(true);
        int totalCoin = GetScore();
        int CountCoin = 0;
        Debug.Log("Score hien tai la: " + totalCoin);
        for(int i = 0;i< totalCoin; i++)
        {
            CountCoin += 1;
            _ToTalCoinText.text = "Score: " + CountCoin;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
    public void ResetCoin()
    {
        coin = 0;
        _CoinText.text = coin.ToString("");
    }
    IEnumerator NPCTalk()
    {
        GameObject checkNPC = GameObject.FindGameObjectWithTag("NPC");
        if(checkNPC != null)
        {
            Debug.Log("Npc ton tai");
            _NpcTalk2.gameObject.SetActive(false);
            _PanelNPCTalk.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _NpcTalk1.gameObject.SetActive(true);
        }
    }
    public void NextTalkButton()
    {
        StartCoroutine(OffTalk1NextTalk2());
    }
    IEnumerator OffTalk1NextTalk2()
    {
        _NpcTalk1.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _NpcTalk2.gameObject.SetActive(true);
    }
    public void OffPanelTalk()
    {
        _PanelNPCTalk.SetActive(false);
    }
}
