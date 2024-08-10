using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Monster2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;//Vị trí của player
    public float speed = 1f;//tốc độ di chuyển của Boss
    public float _Flyspeed = 1.5f;//Tốc độ di chuyển của Boss khi bay
    private Vector3 localScale;//Scale của Boss
    Animator animator;
    [SerializeField] private float _AttackRange;//Phạm vi tấn công
    [SerializeField] private float _DetectionRange;//Phạm vi di chuyển
    private float _AttackBossStart;//Thời gian tấn công của Boss (mặc định là 0)
    [SerializeField] private float _AttackBossCoolDown;//Cool down đoàn tấn công của Boss
    float distancePlayer;//Vị trí player
    Rigidbody2D rb;
    public Slider _hpMonsterSummonSlider;//Thanh slider Hp của Boss
    public int _HpMonsterSummonValue;//Giá trị máu của Boss
    public TextMeshProUGUI _HpMonsterSummonText;
    public GameObject _OffHpSliderEnemy;
    Player _player;
    BoxCollider2D _conllider;
    Rigidbody2D _rb;
    public TextMeshProUGUI _DameText;
    GameController1 _GameControll;
    public GameObject _PanelDame;
    bool isDead;
    AudioSource audioSource;
    public AudioClip audioClipHitEnemy;
    public AudioClip audioClipDead;
    public AudioClip audioClipBossShoutToDead;
    void Start()
    {
        // Lưu trữ scale ban đầu của Boss
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        _AttackBossStart = 0;
        rb=GetComponent<Rigidbody2D>();
        _hpMonsterSummonSlider.maxValue = 300;
        _HpMonsterSummonValue = 300;
        _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString("");
        _player = FindObjectOfType<Player>();
        _conllider = GetComponent<BoxCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _GameControll = FindObjectOfType<GameController1>();
        isDead = false;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        IntroBoss();
        distancePlayer = Vector2.Distance(transform.position, player.position);
        // Tính khoảng cách của nhân vật đến Boss
        Debug.Log("Khoảng cách của player hiện tại là: " + distancePlayer);
        // Xác định hướng của Boss dựa trên vị trí của người chơi
        if (player.position.x > transform.position.x)
        {
            //Thanh Text của Boss sẽ xoay theo tránh trường hợp bị ngược
            _HpMonsterSummonText.transform.localScale = new Vector3(1, 1, 1);
            // Người chơi ở bên phải, quay mặt sang phải
            transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            _HpMonsterSummonText.transform.localScale = new Vector3(-1, 1, 1);
            // Người chơi ở bên trái, quay mặt sang trái
            transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        if (distancePlayer > _DetectionRange)
        {
            //Nếu vị trí của nv lớn hơn phạm vi di chuyển của Boss
            if (_HpMonsterSummonValue > 0)
            {
               //Boss chuyển sang trạng thái bay
                animator.SetTrigger("IsEnemySummonFly");
            }
            MoveBoss();
        }
        else
        {
            if (distancePlayer <= _DetectionRange) // Nếu mà vị trí của người nhỏ hơn phạm vi
                                                   // di chuyển của Boss thì Boss sẽ tiến hành xác định điều kiện tiếp theo
            {
                animator.SetTrigger("IsEnemySummonIdle");
                Debug.Log("Đã phát hiện người chơi nằm trong phạm vi");
                if (distancePlayer > _AttackRange) // Nếu player không nằm trong phạm vi tấn công của Boss
                {
                    animator.SetBool("IsEnemySummonRun", true);
                    Debug.Log("player không nằm trong phạm vi tấn công");
                    MoveBoss();//Boss sẽ di chuyển cho đến khi player nằm trong phạm vi tấn công
                }
                else if (distancePlayer <= _AttackRange) // Người chơi nằm trong phạm vi tấn công
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfo.IsName("ionmySummon_HurtAnimation")) return;
                    //Nếu boss trong trạng thái bị tấn công thì không thể gây dame lên nv
                    else
                    {
                        BossSkillAttack();
                    }
                }

            }
        }
        EnemySummonDead();
    }
    public void IntroBoss()//Intro của Boss khi Boss xuất hiện
    {
        animator.SetTrigger("IsEnemySummonFly");
        rb.velocity = Vector2.down * 1;//Tốc độ đáp xuống của Boss
        if (_conllider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            //Chạm vao Layer "Ground" thì dừng trạng thái bay
            animator.SetTrigger("IsEnemySummonIdle");
            animator.ResetTrigger("IsEnemySummonFly");//Thoát khỏi trạng thái bay
            Debug.Log("Đã va chạm");
            rb.velocity = Vector2.zero;
        }
    }
    public void MoveBoss()
    {
        //animator.SetBool("isBossRun", true);
        // Tính toán hướng tới người chơi nhưng chỉ trên trục X
        if (_HpMonsterSummonValue > 0)
        {
            //biến lấy vị trí của Player
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            // Di chuyển Boss tới vị trí mới
            //Boss di chuyển tới nv bằng phương thức MoveToward
            //MoveToward(vị trí hiện tại của Boss,vị trí mà Boss muốn tới,tốc độ di chuyển của Boss)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (distancePlayer > _DetectionRange)//Nếu mà vị trí của Player lớn hơn pv di chuyển của Boss
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _Flyspeed * Time.deltaTime);
            }            
        }
        else
        {
            return;
        }
    }
    public void BossSkillAttack()
    {
        int randomSkill = Random.Range(0, 3);//Random skill Boss
        int randomdame = Random.Range(10,21 );//Random Dame của Boss
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0&&_HpMonsterSummonValue>0)
        {
            //Boss không nằm trong thời gian cooldown tấn công
            // và máu của Boss lớn hơn 0
            switch (randomSkill)
            {
                case 1:
                    animator.SetTrigger("IsEnemySummonSkill1");
                    _player.TakeDame(randomdame);//Hàm nhận dame của nv
                    break;
                case 2:
                    animator.SetTrigger("IsEnemySummonSkill2");
                    _player.TakeDame(randomdame);
                    break;
            }
            _AttackBossStart = _AttackBossCoolDown;//Reset thời gian tấn công của Boss

        }
        else if (_AttackBossStart > 0) //Nếu còn đang trong thời gian cooldown
        {
            animator.SetTrigger("IsEnemySummonIdle");
            animator.SetBool("IsEnemySummonRun", false);
        }
    }
    public void EnemySummonDead()
    {
        if (_HpMonsterSummonValue <= 0&&isDead==false)
        {
            //Nếu mà hp boss = 0 và biến isdead == false
            StartCoroutine(BossShoutToDead());//Boss chạy sound âm thanh khi bị đánh bại
            isDead = true;//biến isDead = true (bảo đảm boss chỉ chạy hàm này 1 lần)
            _OffHpSliderEnemy.SetActive(false);//Tắt thanh máu của Boss
            Debug.Log("Enemy Da chet");
            animator.ResetTrigger("IsEnemySummonIdle");//Tắt hết các trạng thái của Boss
            animator.ResetTrigger("IsEnemySummonHurt");
            audioSource.PlayOneShot(audioClipDead);
            animator.SetTrigger("IsEnemySummonDead");//Chạy trạng thái chết của Boss
            Destroy(gameObject, 2f);//Phá hủy đối tượng sau 2s
        }
    }
    IEnumerator BossShoutToDead()
    {
        audioSource.PlayOneShot(audioClipBossShoutToDead);
        yield return new WaitForSeconds(0.2f);
    }
    public void StartHurtEnemyAnimation()
    {
        animator.SetTrigger("IsEnemySummonHurt");
    }
    public void StopHurtEnemyAnimation()
    {
        animator.SetTrigger("IsEnemySummonIdle");
    }
    public void EnemySummonTakeDame(int dame)//Hàm nhận dame của Boss
    {
        if (_HpMonsterSummonValue > 0)
        {
            animator.SetTrigger("IsEnemySummonHurt");
            audioSource.PlayOneShot(audioClipHitEnemy);
            _HpMonsterSummonValue -= dame;
            if (_HpMonsterSummonValue <= 0) { _HpMonsterSummonValue = 0; }
            _GameControll.StartDameText(dame, _DameText, gameObject.transform);
            _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString();
            _hpMonsterSummonSlider.value = _HpMonsterSummonValue;
            animator.ResetTrigger("IsEnemySummonSkill1");
            animator.ResetTrigger("IsEnemySummonSkill2");
            Invoke("StopHurtEnemyAnimation", 0.4f);
        }
    }
}
