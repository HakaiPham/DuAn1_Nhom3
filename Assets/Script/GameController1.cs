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
    public TextMeshProUGUI _CoinText;//CoinText
    public static int coin = 0;//Giá trị Coin ban đầu
    public Monster2 monster2Script;//Script của Monster2
    public GameObject _ItemHp;
    public GameObject _ItemMp;
    public GameObject _PanelIntroBoss;
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
        //Lấy tên Scene hiện tại
    }

    // Update is called once per frame
    void Update()
    {
        CheckMonsterDestroy();
        if(currentScene== "Scene4") {//Nếu mà Scene hiện tại là Scene 4
            if (_BossWizard == null && isTotalGameOn == false)
            {//Nếu mà BossWizard == null và Biến này xác định để Bảng tổng kết game chỉ chạy 1 lần
                isTotalGameOn = true;
                StartCoroutine(TotalGame());//Bắt đầu chạy điểm tổng kết game
            }
        }
        if(isNpcTalk == false&& Input.GetKeyDown(KeyCode.B))
        {
            //Nếu mà 
            StartCoroutine(NPCTalk());//Bắt đầu hiện ra cốt truyện
        }
    }
    public void CollectCoin()//Hàm nhận Coin
    {
        coin += 10;
        _CoinText.text = coin.ToString("");
    }
    public static int GetScore()//Hàm nhận coin
    {
        return coin;
    }
    public void CheckMonsterDestroy()
    {
        //Các biến ở dưới check các đối tượng liệu có đang tồn tại trong map hay không
        GameObject[] _Enemy = GameObject.FindGameObjectsWithTag("Monster");
        GameObject _HpItem = GameObject.FindGameObjectWithTag("ItemHp");
        GameObject _MpItem = GameObject.FindGameObjectWithTag("ItemMp");
        GameObject _EnemySummon = GameObject.FindGameObjectWithTag("EnemySummon");
        GameObject _DarkWizard = GameObject.FindGameObjectWithTag("BossTank");
        GameObject _Coin = GameObject.FindGameObjectWithTag("Coin");
        GameObject spawnItemHp;
        GameObject spawnItemMp;
        //Nếu mà 2 Con boss đều là null thì thoát khỏi hàm
        if (_EnemySummon == null && _DarkWizard == null) return;
        Debug.Log("So luong quai con: "+_Enemy.Length);
        if(_Enemy.Length <= 0 && _EnemySummon!=null) {
            //Nếu quái hiện tại trong map đã được Clear
            if (isStartIntroBoss == false)//Thì bắt đầu chạy phần IntroBoss
            {
                StartCoroutine(IntroBoss());//Chạy hàm khởi chạy Intro
                isStartIntroBoss = true;//Hàm chỉ chạy 1 lần
            }
            monster2Script.enabled = true;//Sau khi chạy xong phần Intro thì Bật Script của Boss
            //Random vị trí xuất hiện item
            var randomItemPosition = new Vector2(Random.Range(-7.83f, 13.32f), 0.21f);
            //Random item (hp/mp) được spawn
            int randomItemSpawn = Random.Range(0, 2);
            if (_Coin==null)//Nếu coin = null
            {
                //được phép spawn đồng coin tiếp thep
                var spawnCoin = Instantiate(_CoinSpawn, randomItemPosition, Quaternion.identity);
            }
            if (randomItemSpawn == 0) {//Nếu mà ra số 0 (hp)
                if (_MpItem != null) return;//Xác định không có itemMp nào xuất hiện
                if(_HpItem == null)//Và cũng chưa có bình Hp nào đang tồn tại 
                {
                    //được phép spawn
                    spawnItemHp = Instantiate(_ItemHp, randomItemPosition, Quaternion.identity);
                }
            }
            else if (randomItemSpawn == 1)//Nếu mà ra số 1(mp)
            {
                if (_HpItem != null) return;//Tương tự như hp
                if(_MpItem == null)
                {
                    spawnItemMp = Instantiate(_ItemMp, randomItemPosition, Quaternion.identity);
                }
            }
            //if (_Coin != null) return;
        }
        else if(_DarkWizard != null)//Nếu mà boss Dark wizard khác null
        {
            if (isStartIntroBoss == false)//bắt đầu được phép khởi chạy Intro
            {
                StartCoroutine(IntroBoss2());//Chạy IntroBoss
                isStartIntroBoss = true;//Biến chỉ cho phép chạy 1 lần
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
    {//Hàm tạo ra Dame text trên đầu các đối tượng
        if (monsterTransform.localScale.x < 0)//Dame Text sẽ được xoay theo chiều của đối tượng
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
        yield return new WaitForSeconds(0.1f);//Sau 0.1s thì ẩn đi
        text.gameObject.SetActive(false);

    }
    //hàm khởi chạy DameText
    public void StartDameText(int dame,TextMeshProUGUI text, Transform monsterTransform)
    {
        StartCoroutine(DameText(dame, text, monsterTransform));
    }
    IEnumerator TotalGame()
    {
        //hàm Tổng kết điểm sau khi chiến thắng Boss
        Time.timeScale = 0;//Sau khi chiến thắng thời gian sẽ dừng lại
        _PanelTotalGame.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);//Do Time.timescale = 0 nên ta sẽ
        //lấy thời gian thực để chạy
        _ToTalCoinText.gameObject.SetActive(true);
        int totalCoin = GetScore();//Lấy điểm của các màn chơi
        int CountCoin = 0;//Biến đếm số
        Debug.Log("Score hien tai la: " + totalCoin);
        for(int i = 0;i< totalCoin; i++)//Vòng lặp để tăng điểm
        {
            CountCoin += 1;
            _ToTalCoinText.text = "Score: " + CountCoin;
            yield return new WaitForSecondsRealtime(0.05f);//cứ 0.05s thì tăng 1 điểm
        }
    }
    public void ResetCoin()
    {
        coin = 0;
        _CoinText.text = coin.ToString("");
    }//Reset lại hết tất cả các coin khi mà nhấn chơi lại
    IEnumerator NPCTalk()
    {
        //hàm này chỉ được chạy khi mà trong map có đối tượng có tag là NPC
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
