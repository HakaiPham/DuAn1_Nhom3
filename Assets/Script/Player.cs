using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _MoveSpeed;//Tốc độ di chuyển của nhân vật
    Rigidbody2D _Rigidbody2;
    BoxCollider2D _Collider2;
    public Animator _Animator2;
    [SerializeField] private float _JumpPower;//Lực nhảy của nhân vật
    [SerializeField] private float _JumpPowerSkill;//lực nhảy của skillă
    CircleCollider2D circleCollider;
    [SerializeField] private float _AttackRange;//Phạm vi tấn công
    public Transform[] _monster;//Mảng chứa số lượng quái có trong mảng
    [SerializeField] private Transform _TransformAttack;//Vị trí tấn công quái
    public GameObject _Bullet;//Đạn
     public Slider _HpSlider;//Thanh slider HP
     public Slider _MpSlider;// Thanh Slider MP
    public int hpValue;//HpValue
    [SerializeField] public  TextMeshProUGUI _HpText;
    [SerializeField] public TextMeshProUGUI _MpText;
    public int mpValue;
    public Image _Skill1Image;//Ảnh skill1
    public Image _Skill2Image;//Ảnh skill2
    Monster _HpMonster;//Script của Monster
    bool isMonsterinRange;//Biến xác định liệu monster có trong phạm vi không
    float positionMonster;//vị trí của monster
    [SerializeField] private GameObject _HpItem;
    [SerializeField] private GameObject _MpItem;
    [SerializeField] public TextMeshProUGUI _SlHpText;
    int slHp = 0;
    int slMp = 0;
    [SerializeField] public TextMeshProUGUI _SlMpText;
    bool canUseItem;//Biến xác định có được phép sử dụng Item không
    public Transform targetMonster;// Transform của monster
    Monster2 _EnemySummon;//Script của Boss 1
    BossTank _BossTank;//Sciprt của Boss 2
    bool isDead;//Biến xác định nhân vật đã chết chưa
    private float _TimeAttackStart;//Thời gian bắt đầu(Mặt định là 0)
   [SerializeField] private float cooldownAttackTime;//thời gian cooldown Attack
    Enemy2 _enemy2;//Script của Enemy
    Enemy3 _enemy3;
    EnermyArrow _enemyArrow;
    Enemy4 _enemy4;
    Enemy5 _enemy5;
    Enemy6 _enemy6;
    Enemy7 _enemy7;
    Enemy8 _enemy8;
    Enemy9 _enemy9;
    GameController1 _GameController1;
    public TextMeshProUGUI _DameTextPlayer;
    AudioSource audioSource;
    public AudioClip audioClipAttack;
    public AudioClip audioClipHurt;
    public AudioClip audioClipCollectCoin;
    public AudioClip audioClipDead;
    public AudioClip audioClipJump;
    public AudioSource _MusicGameSource;
    void Start()
    {
        _Rigidbody2 = GetComponent<Rigidbody2D>();
        _Collider2 = GetComponent<BoxCollider2D>();
        _Animator2 = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        _HpSlider.maxValue = 100;
        hpValue = 100;
        _HpText.text = hpValue.ToString("");
        _MpSlider.maxValue = 100;
        mpValue = 100;
        _MpText.text = mpValue.ToString("");
        _HpMonster = FindFirstObjectByType<Monster>();
        _SlHpText.text = slHp.ToString("");
        _SlMpText.text = slMp.ToString("");
        _EnemySummon = FindObjectOfType<Monster2>();
        _BossTank = FindObjectOfType<BossTank>();//Tìm kiếm đối tượng mang Script BossTank
        _enemy2 = FindObjectOfType<Enemy2>();
        isDead = false;
        _TimeAttackStart = 0;
        _enemy3 = FindObjectOfType<Enemy3>();
        _enemyArrow = FindObjectOfType<EnermyArrow>();
        _enemy5 = FindObjectOfType<Enemy5>();
        _enemy4 = FindObjectOfType<Enemy4>();
        _enemy6 = FindObjectOfType<Enemy6>();
        _enemy7 = FindObjectOfType<Enemy7>();
        _enemy8 = FindObjectOfType<Enemy8>();
        _enemy9 = FindObjectOfType<Enemy9>();
        _GameController1 = FindObjectOfType<GameController1>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Jump();
        PlayerAttack();
        //SkillAttack1();
        if (canUseItem && Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Nếu CanUseItem == true và Người chơi nhấn phím 1
            if (slHp > 0)//Nếu số lượng item Hp >0
            {
                slHp -= 1;//Sau khi dùng thì sl item -1
                _Animator2.SetBool("IsHealth", true);//Chạy Animation
                Invoke("AnimationEatHpFinished", 0.5f);//Đợi 0,5f để animation chạy xong sẽ chạy hàm
            }
        }
        else if (canUseItem && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slMp > 0)
            {
                slMp -= 1;
                _Animator2.SetBool("IsMana", true);
                Invoke("AnimationEatMpFinished", 0.5f);
            }
        }
        if (hpValue <= 0 && isDead == false)//Nếu hp của nv <=0 và biến is Dead ==false
        {
            isDead = true;//biến isDead == true(tránh trường hợp nhân vật lặp lại hiệu ứng chết)
            StartCoroutine(OffMusicGame());//Đợi 1 khoảng thời gian rồi tắt nhạc game
            Dead();//Chạy dòng code nhân vật chết
        }
        PlayerClimp();
    }
    public void AnimationEatHpFinished()
    {
       
            _SlHpText.text = slHp.ToString();
            StartCoroutine(EatHpItem());//Chạy code tăng máu 
            _Animator2.SetBool("IsHealth", false);// tắt animation       
    }
    public void AnimationEatMpFinished()
    {

        _SlMpText.text = slMp.ToString();
        StartCoroutine(EatMpItem());
        _Animator2.SetBool("IsMana", false);

    }
    public void PlayerAttack()
    {
        _TimeAttackStart -= Time.deltaTime;
        isMonsterinRange = false; // quái chưa có vào phạm vi tấn công
        targetMonster = null;//Bắt đầu cho Transform bằng null
        Debug.Log("Số lượng quái có trong mảng là: " + _monster.Length);
        foreach(Transform monster in _monster) //Duyệt tất cả các quái có trong mảng
        {
            if (monster != null) // Kiểm tra nếu monster không phải là null
            {
                positionMonster = Vector2.Distance(transform.position, monster.position);
                //Vị trí tính toán khoản khách từ nv đến các quái có trong map
                if (positionMonster <= _AttackRange)//Quái đang nằm trong pv tấn công của quái
                {
                    isMonsterinRange = true;//Quái đang nằm trong phạm vi tấn công
                    targetMonster = monster; // Lưu lại quái vật trong phạm vi tấn công
                    break; // Thoát khỏi vòng lặp nếu tìm thấy quái vật trong phạm vi
                }
            }
        }
        if (isMonsterinRange==true&&_TimeAttackStart<=0)
        {
            //Quái đang nằm trong pv tấn công và thời gian tấn công hiện tại <=0
            if (Input.GetKeyDown(KeyCode.E))//Nhấn E
            {
                audioSource.PlayOneShot(audioClipAttack);//Chạy sound tấn công
                _Animator2.SetTrigger("IsAttack");//Animation tấn công
                AttackMonsterbyNormalAttack(targetMonster);//Hàm trừ máu
                _TimeAttackStart = cooldownAttackTime;//Reset lại thời gian tấn công
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F)&&_MpSlider.value>=10
                    &&_Skill1Image.fillAmount>=1)
                {
                    //Nhấn F và Mp hiện tại phải >=10 và skill 1 không có trong thời gian cooldown
                    SkillAttack1();//nv bay lên và chạy animation
                    _MpSlider.value -= 10;//trừ 10 mp
                    mpValue -= 10;
                    _MpText.text = mpValue.ToString("");
                    _Skill1Image.fillAmount = 0;//skill1 đang trong thời gian cooldown
                    //AttackMonsterbySkill1();
                    StartCoroutine(ReLoadSkill1());//thời gian cooldown skill1
                    _TimeAttackStart = cooldownAttackTime;//reset lại thời gian tấn công
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.R)&&_MpSlider.value>=20
                        &&_Skill2Image.fillAmount>=1)
                    {
                        OnSkill2();
                        _MpSlider.value -= 20;
                        mpValue -= 20;
                        _MpText.text = mpValue.ToString("");
                        Invoke("CreateBullet",0.47f);
                        _Skill2Image.fillAmount = 0;
                        _TimeAttackStart = cooldownAttackTime;
                    }
                }
            }
        }
        else
        {
            //Các hàm bên dưới ngoại trừ chiêu R thì 2 chiêu còn lại không thể gây dame
            //khi ở ngoại phạm vi tấn công
            if (isMonsterinRange == false&&_TimeAttackStart<=0)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    audioSource.PlayOneShot(audioClipAttack);
                    _Animator2.SetTrigger("IsAttack");
                    _TimeAttackStart = cooldownAttackTime;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F) && _MpSlider.value >= 10
                        && _Skill1Image.fillAmount >= 1)
                    {
                        SkillAttack1();
                        _MpSlider.value -= 10;
                        mpValue -= 10;
                        _MpText.text = mpValue.ToString("");
                        _Skill1Image.fillAmount = 0;
                        StartCoroutine(ReLoadSkill1());
                        _TimeAttackStart = cooldownAttackTime;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.R) && _MpSlider.value >= 20
                            && _Skill2Image.fillAmount >= 1)
                        {
                            OnSkill2();
                            _MpSlider.value -= 20;
                            mpValue -= 20;
                            _MpText.text = mpValue.ToString("");
                            Invoke("CreateBullet", 0.47f);
                            _Skill2Image.fillAmount = 0;
                            _TimeAttackStart = cooldownAttackTime;
                        }
                    }
                }
            }
        }
        //StopAttack();
    }
    public void TakeDame(int dame)//Hàm nhận dame
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        //Lấy animation hiện tại tại Layer 0
        if (stateInfo.IsName("Player_Skill1Animation") ||
            stateInfo.IsName("Player_Skill1_2Animation")) return;
        //Nếu player đang trong trạng thái skill 1 thì không thể bị mất máu
        if (hpValue >= 0)//hp player >=0 thì mơi có thể nhận dame
        {
            hpValue -= dame;
            if (hpValue <= 0)//điều kiện tránh trường hợp bị âm máu
            {
                hpValue = 0;//hp của nv trả về 0 ngay khi bị trừ hết máu
            }
            //hiện hiệu ứng gây dame ngay trên đầu nhân vật
                _GameController1.StartDameText(dame, _DameTextPlayer,gameObject.transform);
            _HpSlider.value = hpValue;
            _HpText.text = hpValue.ToString("");
            audioSource.PlayOneShot(audioClipHurt);//sound bị tấn công
            _Animator2.SetTrigger("IsHurt");//animation Hurt
            Invoke("StopHurt", 0.2f);//Sau 0.2f thì chuyển từ trạng thái "Hurt"
            //sang trạng thái Idle tránh trường hợp bị lặp lại nhiều lần
        }
    }
    public void StopHurt()//Hàm chuyển sang trạng thái idle
    {
        _Animator2.SetTrigger("IsIdle");
    }
    IEnumerator ReLoadSkill1()//Hàm cooldown skill1
    {
        for(int i = 0; i < 10; i++)
        {
            _Skill1Image.fillAmount += 0.1f;
            yield return new WaitForSeconds(0.5f);//Cử 0.5 s là +0.1
        }
    }
    IEnumerator ReLoadSkill2()//Hàm reload skill 2
    {
        for (int i = 0; i < 10; i++)
        {
            _Skill2Image.fillAmount += 0.1f;
            yield return new WaitForSeconds(0.6f);//Cứ 0.6s là +0.1
        }
    }
    public void CreateBullet()//Hàm tạo ra viên đạn
    {
        //biến khởi tạo ra viên đạn
        var createBullet = Instantiate(_Bullet, _TransformAttack.position, Quaternion.identity);
        //lấy tên Scene hiện tại
        var currentScene = SceneManager.GetActiveScene().name;
        if (transform.localScale.x > 0)//Nếu scale quay sang phải
        {
            var transformAttack = new Vector2(3f, 0);//tốc độ của viên đạn
            if(currentScene != "Scene4")//Ngoại trừ Scene 4 thì viên đạn sẽ có kích thước là 2
            {
                createBullet.transform.localScale = new Vector3(2, 2, 2);
            }
            else if(currentScene == "Scene4") 
                //Ngược lại thì ở Scene 4 thì viên đạn sẽ có kích thước là 3
            {
                createBullet.transform.localScale = new Vector3(3, 3, 3);
            }
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
            //Chuyển động vật lý của viên đạn
        }
        else if (transform.localScale.x < 0)
        {
            var transformAttack = new Vector2(-3f,0);
            if (currentScene != "Scene4")
            {
                createBullet.transform.localScale = new Vector3(-2, -2, -2);
            }
            else if (currentScene == "Scene4")
            {
                createBullet.transform.localScale = new Vector3(-3, -3, 3);
            }
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        StartCoroutine(ReLoadSkill2());//Thời gian Cooldown của Skill2
        Destroy(createBullet,2f);//Phá hủy viên đạn sau 2s

    }
    public void MovePlayer()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        _Animator2.SetBool("IsRun", true);
        var playerposition = transform.position;
        var localscale = transform.localScale;
        if (horizontalInput > 0 || horizontalInput < 0)
        {
            //Ở dòng code phía dưới cho phép tùy chỉnh kích thước nhân vật
            //Tránh trường hợp nhân vật quá nhỏ hoặc quá lớn
            localscale.x = Mathf.Abs(transform.localScale.x) * Mathf.Sign(horizontalInput);
            transform.localScale = localscale;
        }
        if (horizontalInput == 0)
        {
            _Animator2.SetBool("IsRun", false);
        }
        transform.localPosition += new Vector3(horizontalInput, 0, 0) * _MoveSpeed * Time.deltaTime;
    }
    public void Jump()
    {
        if (!_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(audioClipJump);
                _Animator2.SetTrigger("IsJump");
                _Rigidbody2.velocity = new Vector2(_Rigidbody2.velocity.x, _JumpPower);
            }
            else
            {
                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    _Animator2.SetBool("IsIdle", true);
                }
            }
        }
    }
    public void Dead()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        _Collider2.enabled = false;//Đảo ngước trạng thái của Conllider bật tắc
            circleCollider.enabled = true;
        _Rigidbody2.gravityScale = 1;
        if (!stateInfo.IsName("Player_DeadAnimation")){
            _Animator2.ResetTrigger("IsHurt");
            _Animator2.ResetTrigger("IsIdle");
            _Animator2.SetTrigger("IsDead");
        }          
    }
    public void SkillAttack1()
    {
        // Bắt đầu animation skill
        _Animator2.SetTrigger("IsSkill1");//Hiệu ứng nhảy lên của skill1
        _Rigidbody2.velocity = Vector2.up * _JumpPowerSkill;
    }
    public void OnSkill1End()//Kết thúc skill1(Event)
    {
        // Animation event khi skill 1 kết thúc
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground"))
            || _Collider2.IsTouchingLayers(LayerMask.GetMask("Monster")))
        {
            //Conllider của nv khi va chạm với layer "Ground" và "Monster"
            _Animator2.SetTrigger("IsSkill1_2");//Chạy Animation còn lại của skill1
            AttackMonsterbySkill1(targetMonster);//Hàm trừ máu của skill1
        }
        else
        {
            //Tránh trường hợp chưa chạm đất thì chạy animation tiếp theo
            _Animator2.ResetTrigger("IsSkill1_2");//Ngược lại thì ResetTrigger
        }
    }
    public void OnSkill2()
    {
           _Animator2.SetTrigger("IsAttackSkill2");
    }
    public void OnSkill2End()
    {
        _Animator2.SetTrigger("IsIdle");
    }
    public void PlayerClimp()
    {
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Stair")))
        {
            //Nếu nv va chạm với cái Layer có tên là Stair
            _Rigidbody2.gravityScale = 0;//trọng lực  = 0 
            if (Input.GetKey(KeyCode.DownArrow))
            {
                //Nếu mà người chơi nhân nút mũi tên
                _Animator2.SetBool("IsClimp", true);//Hiệu ứng leo
                _Rigidbody2.velocity = Vector2.down * 1;//Tốc độ leo
            }
            else if (!Input.GetKey(KeyCode.DownArrow))
            {
                //ngược lại nếu không bấm thì vấn tộc =0
                _Rigidbody2.velocity = Vector2.zero;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _Animator2.SetBool("IsClimp", true);
                _Rigidbody2.velocity = Vector2.up * 1;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                _Rigidbody2.velocity = Vector2.zero;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ItemHp"))
        {
            //Nếu mà người chơi va chạm với tag có tên là "ItemHp"
            Destroy(collision.gameObject);//Phá hủy đối tượng đó
            slHp += 1;//số lượng item +1
            _SlHpText.text = slHp.ToString("");
            canUseItem = true;//khi mà sl item khác không thì người chơi được phép sử dụng item
        }
        if (collision.gameObject.CompareTag("ItemMp"))
        {
            Destroy(collision.gameObject);
            slMp += 1;
            _SlMpText.text = slMp.ToString("");
            canUseItem = true;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(audioClipCollectCoin);
            _GameController1.CollectCoin();
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            //Khi mà player rời khỏi không còn va chạm với cái tag đó 
            //thì chạy dòng code bên dưới
            _Rigidbody2.gravityScale = 1;
            _Animator2.SetBool("IsClimp", false);
        }
    }
    private void AttackMonsterbyNormalAttack(Transform monster)
    {
        //Lấy Transform của các Enemy
        if (monster == null) return;//Nếu mà monster == null thì kết thúc

        _HpMonster = monster.GetComponent<Monster>();//Lấy Tranform của Enemy
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        _enemyArrow = monster.GetComponent<EnermyArrow>();
        _enemy4 = monster.GetComponent<Enemy4>();
        _enemy5 = monster.GetComponent<Enemy5>();
        _enemy6 = monster.GetComponent<Enemy6>();
        _enemy7 = monster.GetComponent<Enemy7>();
        _enemy8 = monster.GetComponent<Enemy8>();
        _enemy9 = monster.GetComponent<Enemy9>();
        if (positionMonster <= _AttackRange)
        {
            //nếu Enemy nằm trong phạm vi tấn công
            int randomDame = Random.Range(5, 11);//Random dame cho đánh thường
            if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
            {
                //Nếu mà đối tượng Enemy khác null và máu của enemy phải lớn hơn 0
                _HpMonster.TakeDameEnemy(randomDame);//Hàm nhận dame của Enemy
            }
            else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
            {
                _EnemySummon.EnemySummonTakeDame(randomDame);
            }
            if (_BossTank != null && _BossTank._HpBossTankValue > 0)
            {
                _BossTank.TakeDameBoss(randomDame);
            }
            else
            {
                if (_enemy2 != null && _enemy2.hpEmenyValue > 0) _enemy2.Enemy2TakeDame(randomDame);
            }
            if (_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(randomDame);
            else if (_enemyArrow != null && _enemyArrow.hp > 0) _enemyArrow.TakeDamge(randomDame);
            if(_enemy4 != null && _enemy4.hpEmenyValue > 0) _enemy4.Enemy4TakeDame(randomDame);
            else if(_enemy5 != null && _enemy5.hpEmenyValue > 0) _enemy5.Enemy5TakeDame(randomDame);
            if (_enemy6 != null && _enemy6.hpEmenyValue > 0) _enemy6.Enemy6TakeDame(randomDame);
            else if (_enemy7 != null && _enemy7.hpEmenyValue > 0) _enemy7.Enemy7TakeDame(randomDame);
            if (_enemy8 != null && _enemy8.hpEmenyValue > 0) _enemy8.Enemy8TakeDame(randomDame);
            else if (_enemy9 != null && _enemy9.hpEmenyValue > 0) _enemy9.Enemy9TakeDame(randomDame);
        }
    }
    private void AttackMonsterbySkill1(Transform monster)
    {
        if (monster == null) return;

        _HpMonster = monster.GetComponent<Monster>();
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        _enemyArrow = monster.GetComponent<EnermyArrow>();
        _enemy4 = monster.GetComponent<Enemy4>();
        _enemy5 = monster.GetComponent<Enemy5>();
        _enemy6 = monster.GetComponent<Enemy6>();
        _enemy7 = monster.GetComponent<Enemy7>();
        _enemy8 = monster.GetComponent<Enemy8>();
        _enemy9 = monster.GetComponent<Enemy9>();
        if (positionMonster <= _AttackRange)
        {
            if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
            {
                _HpMonster.TakeDameEnemy(20);
            }
            else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
            {
                _EnemySummon.EnemySummonTakeDame(20);
            }
            if (_BossTank != null && _BossTank._HpBossTankValue > 0)
            {
                _BossTank.TakeDameBoss(20);
            }
            else
            {
                if (_enemy2 != null && _enemy2.hpEmenyValue > 0) _enemy2.Enemy2TakeDame(20);
            }
            if (_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(20);
            else if (_enemyArrow != null && _enemyArrow.hp > 0) _enemyArrow.TakeDamge(20);
            if (_enemy4 != null && _enemy4.hpEmenyValue > 0) _enemy4.Enemy4TakeDame(20);
            else if (_enemy5 != null && _enemy5.hpEmenyValue > 0) _enemy5.Enemy5TakeDame(20);
            if (_enemy6 != null && _enemy6.hpEmenyValue > 0) _enemy6.Enemy6TakeDame(20);
            else if (_enemy7 != null && _enemy7.hpEmenyValue > 0) _enemy7.Enemy7TakeDame(20);
            if (_enemy8 != null && _enemy8.hpEmenyValue > 0) _enemy8.Enemy8TakeDame(20);
            else if (_enemy9 != null && _enemy9.hpEmenyValue > 0) _enemy9.Enemy9TakeDame(20);
        }
    }
    public void AttackMonsterbySkill2(Transform monster)
    {
        if (monster == null) return;
        _HpMonster = monster.GetComponent<Monster>();
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        _enemy4 = monster.GetComponent<Enemy4>();
        _enemy5 = monster.GetComponent<Enemy5>();
        _enemy6 = monster.GetComponent<Enemy6>();
        _enemy7 = monster.GetComponent<Enemy7>();
        _enemy8 = monster.GetComponent<Enemy8>();
        _enemy9 = monster.GetComponent<Enemy9>();
        if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
        {
            _HpMonster.TakeDameEnemy(50);
        }
        else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
        {
            _EnemySummon.EnemySummonTakeDame(50);
        }
        if (_BossTank != null)
        {
            _BossTank.TakeDameBoss(50);
        }
        else
        {
            if (_enemy2 != null) _enemy2.Enemy2TakeDame(50);
        }
        if (_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(50);
        else if (_enemy5 != null && _enemy5.hpEmenyValue > 0) _enemy5.Enemy5TakeDame(50);
        if (_enemy4 != null && _enemy4.hpEmenyValue > 0) _enemy4.Enemy4TakeDame(50);
        else if (_enemy6 != null && _enemy6.hpEmenyValue > 0) _enemy6.Enemy6TakeDame(50);
        if (_enemy7 != null && _enemy7.hpEmenyValue > 0) _enemy7.Enemy7TakeDame(50);
        else if (_enemy8 != null && _enemy8.hpEmenyValue > 0) _enemy8.Enemy8TakeDame(50);
        else if (_enemy9 != null && _enemy9.hpEmenyValue > 0) _enemy9.Enemy9TakeDame(50);
    }
    IEnumerator EatHpItem()
    {
        Debug.Log("--------------");
        for(int i = 0; i < 20; i++)
        {
            if (hpValue>=0&&hpValue <100)
            {
                if (hpValue == 100) //Nếu giá trị máu hiện tại = 100
                {
                    break;//thoát khỏi vòng lặp
                }
                hpValue += 1;
                _HpText.text = hpValue.ToString("");
                _HpSlider.value = hpValue;
                yield return new WaitForSeconds(0.3f);//Cứ mỗi 0.3s là + 1 máu
            }
        }
    }
    IEnumerator EatMpItem()
    {
        Debug.Log("--------------");
        for (int i = 0; i < 20; i++)
        {
            if (mpValue >= 0 && mpValue < 100)
            {
                if (mpValue == 100)
                {
                    break;
                }
                mpValue += 1;
                _MpText.text = mpValue.ToString("");
                _MpSlider.value = mpValue;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    IEnumerator OffMusicGame()//Tắt nhạc nền game
    {
        _MusicGameSource.Stop();
        yield return new WaitForSecondsRealtime(0.1f);
        audioSource.PlayOneShot(audioClipDead);
    }
}
