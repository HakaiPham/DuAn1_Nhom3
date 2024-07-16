using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossTank : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public float speed = 1f;
    private Vector3 localScale;
    Animator animator;
    [SerializeField] private float _AttackRange;
    [SerializeField] private float _DetectionRange;
    private float _AttackBossStart;
    [SerializeField] private float _AttackBossCoolDown;
    float distancePlayer;
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    [SerializeField] private GameObject _BossBullet;
    public Transform _BossAttackTransform;
    [SerializeField] private GameObject _Metorite;
    [SerializeField] private GameObject _GateSummon;
    public float skill1Cooldown = 1f; // Cooldown cho skill đánh thường
    public float skill2Cooldown = 20f; // Cooldown cho skill 2
    public float skill3Cooldown = 15f; // Cooldown cho skill 3
    public float skill4Cooldown = 10f;// Cooldown cho skill4
    private float nextSkill1Time;
    private float nextSkill2Time;
    private float nextSkill3Time;
    private float nextSkill4Time;
    bool isTele;
    public GameObject _Lazer;
    public Slider hpBossTankSlider;
    private int _HpBossTankValue;
    public TextMeshProUGUI _HpText;
    public GameObject _OffSlider;
    void Start()
    {
        // Lưu trữ scale ban đầu của Boss
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        _AttackBossStart = 0;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hpBossTankSlider.maxValue = 500;
        _HpBossTankValue = 500;
        _HpText.text = _HpBossTankValue.ToString("");
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position);
        // Tính khoảng cách của nhân vật đến Boss
        Debug.Log("Khoảng cách của player hiện tại là: " + distancePlayer);
        if (distancePlayer > _DetectionRange)
        {
            MoveBoss();
        }
        else
        {
            if (distancePlayer <= _DetectionRange) // Nếu mà vị trí của người nhỏ hơn phạm vi
                                                   // di chuyển của Boss thì Boss sẽ tiến hành xác định điều kiện tiếp theo
            {
                animator.SetBool("IsBossRun", false);
                Debug.Log("Đã phát hiện người chơi nằm trong phạm vi");
                if (distancePlayer > _AttackRange) // Nếu player không nằm trong phạm vi tấn công của Boss
                {
                    Debug.Log("player không nằm trong phạm vi tấn công");
                    MoveBoss();//Boss sẽ di chuyển cho đến khi player nằm trong phạm vi tấn công
                }
                else if (distancePlayer <= _AttackRange) // Người chơi nằm trong phạm vi tấn công
                {
                    BossSkillAttack();
                }

            }
        }
    }
    public void MoveBoss()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Boss_TeleStartAnimation")||
           stateInfo.IsName("Boss_teleAnimation")) { return; }
        //animator.SetBool("isBossRun", true);
        // Tính toán hướng tới người chơi nhưng chỉ trên trục X
        else
        {
            animator.SetBool("IsBossRun", true);
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

            // Di chuyển Boss tới vị trí mới
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Xác định hướng của Boss dựa trên vị trí của người chơi
            if (player.position.x > transform.position.x)
            {
                // Người chơi ở bên phải, quay mặt sang phải
                transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
            else if (player.position.x < transform.position.x)
            {
                // Người chơi ở bên trái, quay mặt sang trái
                transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
        }
    }
    public int randomskillPercent()
    {
        int randomSkill = Random.Range(0, 101);
        if (randomSkill <= 50)
        {
            // 60% xác suất cho skill 1 (đánh thường)
            return 1;
        }
        else if (randomSkill <= 70)
        {
            // 10% xác suất cho skill 3
            return 3;
        }
        if (randomSkill <= 90)
        {
            return 2;
        }
        else
        {
            return 4;
        }
    }
    public void BossSkillAttack()
    {
        int skill = randomskillPercent();
        Debug.Log("Hiện tại skill nằm trong là: " + skill);
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0)
        {
            Debug.Log("Dã chạy");
            switch (skill)
            {
                case 1:
                    if(Time.time >= nextSkill1Time)
                    {
                        Debug.Log(">>>>>>>>>>>>>1");
                        animator.SetBool("IsBossAttack", true);
                        nextSkill1Time = Time.time + skill1Cooldown;
                    }                  
                    break;
                case 2:
                    if (Time.time >= nextSkill2Time)
                    {
                        Debug.Log(">>>>>>>>>>>>>2");
                        animator.SetBool("IsBossAttack2", true);
                        nextSkill2Time = Time.time + skill2Cooldown;
                    }                   
                    break;
                case 3:
                    if(Time.time >= nextSkill3Time)
                    {
                        animator.SetBool("IsBossAttack3", true);
                        nextSkill3Time = Time.time + skill3Cooldown;
                    }
                    break;
                case 4:
                    if (Time.time >= nextSkill4Time)
                    {
                        StartCoroutine(TeleportBoss());
                        nextSkill4Time = Time.time + skill4Cooldown;
                    }
                    break;
            }
            _AttackBossStart = _AttackBossCoolDown;
        }
    }
    public void BossNormalAttack()
    {
        Debug.Log("Đã tạo ra viên đạn");
        var createBullet = Instantiate(_BossBullet, _BossAttackTransform.position, Quaternion.identity);
        var speedBullet = new Vector2(3f, 0);
        createBullet.transform.localScale = new Vector3(6, 6, 6);
        if (transform.localScale.x < 0)
        {
            speedBullet = new Vector2(-2f, 0);
            createBullet.transform.localScale = new Vector3(-6, -6, -6);
        }
        createBullet.GetComponent<Rigidbody2D>().velocity = speedBullet;
        Destroy(createBullet, 2f);
    }
    public void EndNormalAttack()
    {
        animator.SetBool("IsBossAttack", false);
    }
    public void EndSkill1()
    {
        animator.SetBool("IsBossAttack2", false);
    }
    public void EndSkill2()
    {
        animator.SetBool("IsBossAttack3", false);
    }
    public void CreateMetorite()//Đạn cho skill 
    {
        StartCoroutine(WaitCreateMetorite());
    }
    IEnumerator WaitCreateMetorite()
    {
        for (int i = 0; i < 10; i++)
        {
            var randomPostion = new Vector2(Random.Range(-7.69f, 7.32f), 6);
            var createMetorite = Instantiate(_Metorite, randomPostion, Quaternion.Euler(0, 0, -90.322f));
            var speedfall = new Vector2(0, -3f);
            createMetorite.GetComponent<Rigidbody2D>().velocity = speedfall;
            yield return new WaitForSeconds(1.5f);
        }
    }
    public void CreateGateSummon()
    {
        var randomPostion = new Vector2(Random.Range(-7.59f, 6.17f), 0.66f);
        var createMetorite = Instantiate(_GateSummon, randomPostion, Quaternion.identity);
        Destroy(createMetorite, 3f);
    }
    private IEnumerator TeleportBoss()
    {
        isTele = true; // Bắt đầu quá trình teleport
        animator.SetTrigger("IsBossStartTele");

        // Chờ một chút để đảm bảo animator đã cập nhật
        yield return new WaitForSeconds(0.1f);

        // Kiểm tra và chờ cho đến khi animation bắt đầu
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Boss_TeleStartAnimation"))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        // Chờ cho đến khi animation hoàn thành
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        // Chờ một chút để tránh giật sau khi teleport
        yield return new WaitForSeconds(0.1f);
        // Hoàn thành quá trình teleport
        animator.SetTrigger("IsBossFinishedTele");
        // Thực hiện teleport
        var positionBossTele = new Vector2(Random.Range(-7.84f, 7.60f), -0.47f);
        transform.position = positionBossTele;
        isTele = false;
    }
    public void EndSkillBossTele()
    {
        animator.SetTrigger("IsBossIdle");
    }
    public void CreateLaze()
    {

        StartCoroutine(LazerSpawn());
    }
    IEnumerator LazerSpawn()
    {
        for (int i = 0; i < 10; i++)
        {
            var spawnLazerPosition = new Vector2(Random.Range(-7.3f, 7.8f), 1.6f);
            var createLaze = Instantiate(_Lazer, spawnLazerPosition, Quaternion.Euler(0,0,90));
            createLaze.transform.localScale = new Vector2(18.16f, 8.218f);
            yield return new WaitForSeconds(1f);
        }
    }
    public void TakeDameBoss(int dame)
    {
        animator.SetTrigger("IsBossHurt");
        _HpBossTankValue -= dame;
        hpBossTankSlider.value = _HpBossTankValue;
        _HpText.text = _HpBossTankValue.ToString("");
        if (_HpBossTankValue <= 0)
        {
            _OffSlider.SetActive(false);
            animator.SetTrigger("IsBossDead");
        }
    }
}
