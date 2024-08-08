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
    public Transform player;
    public float speed = 1f;
    public float _Flyspeed = 1.5f;
    private Vector3 localScale;
    Animator animator;
    [SerializeField] private float _AttackRange;
    [SerializeField] private float _DetectionRange;
    private float _AttackBossStart;
    [SerializeField] private float _AttackBossCoolDown;
    float distancePlayer;
    Rigidbody2D rb;
    public Slider _hpMonsterSummonSlider;
    public int _HpMonsterSummonValue;
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
            if (_HpMonsterSummonValue > 0)
            {
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
                    else
                    {
                        BossSkillAttack();
                    }
                }

            }
        }
        EnemySummonDead();
    }
    public void IntroBoss()
    {
        animator.SetTrigger("IsEnemySummonFly");
        rb.velocity = Vector2.down * 1;
        if (_conllider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            animator.SetTrigger("IsEnemySummonIdle");
            animator.ResetTrigger("IsEnemySummonFly");
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
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            // Di chuyển Boss tới vị trí mới
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (distancePlayer > _DetectionRange)
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
        int randomSkill = Random.Range(0, 3);
        int randomdame = Random.Range(10,21 );
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0&&_HpMonsterSummonValue>0)
        {
            switch (randomSkill)
            {
                case 1:
                    animator.SetTrigger("IsEnemySummonSkill1");
                    _player.TakeDame(randomdame);
                    break;
                case 2:
                    animator.SetTrigger("IsEnemySummonSkill2");
                    _player.TakeDame(randomdame);
                    break;
            }
            _AttackBossStart = _AttackBossCoolDown;

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
            StartCoroutine(BossShoutToDead());
            isDead = true;
            _OffHpSliderEnemy.SetActive(false);
            Debug.Log("Enemy Da chet");
            animator.ResetTrigger("IsEnemySummonIdle");
            animator.ResetTrigger("IsEnemySummonHurt");
            audioSource.PlayOneShot(audioClipDead);
            animator.SetTrigger("IsEnemySummonDead");
            Destroy(gameObject, 2f);
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
    public void EnemySummonTakeDame(int dame)
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
