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
    public float skill2Cooldown = 5f; // Cooldown cho skill 2
    public float skill3Cooldown = 10f; // Cooldown cho skill 3
    public float skill4Cooldown = 10f;// Cooldown cho skill4
    private float nextSkill1Time;
    private float nextSkill2Time;
    private float nextSkill3Time;
    private float nextSkill4Time;
    bool isTele;
    public GameObject _Lazer;
    public Slider hpBossTankSlider;
    public int _HpBossTankValue;
    public TextMeshProUGUI _HpText;
    public GameObject _OffSlider;
    bool isDead;
    bool _CanUseSkillTele;
    private bool isPreparingToTeleport = false;
    public TextMeshProUGUI _DameText;
    GameController1 gameController1;
    AudioSource audioSource;
    public AudioClip audioClipHitEnemy;
    public AudioClip audioClipDead;
    public AudioClip audioClipLazedSkill;
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
        isDead = false;
        _CanUseSkillTele = false;
        gameController1 = FindObjectOfType<GameController1>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position);
        // Tính khoảng cách của nhân vật đến Boss
        Debug.Log("Khoảng cách của player hiện tại là: " + distancePlayer);
        if (player.position.x > transform.position.x)
        {
            _HpText.transform.localScale = new Vector3(1, 1, 1);
            // Người chơi ở bên phải, quay mặt sang phải
            transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            _HpText.transform.localScale = new Vector3(-1, 1, 1);
            // Người chơi ở bên trái, quay mặt sang trái
            transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        if (distancePlayer > _AttackRange)
        {
            MoveBoss();
        }
        else
        {
            if (distancePlayer <= _AttackRange) // Nếu mà vị trí của người nhỏ hơn phạm vi
                                                   // di chuyển của Boss thì Boss sẽ tiến hành xác định điều kiện tiếp theo
            {
                animator.SetBool("IsBossRun", false);
                BossSkillAttack();

            }
        }
        if (_HpBossTankValue <= 0 && isDead == false)
        {
            _OffSlider.SetActive(false);
            audioSource.PlayOneShot(audioClipDead);
            animator.SetTrigger("IsBossDead");
            isDead = true;
            Destroy(gameObject, 2f);
        }
    }
    public void MoveBoss()
    {
        if (_HpBossTankValue <= 0) return;
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
        }
    }
    public int randomskillPercent()
    {
        int randomSkill = Random.Range(0, 101);
        if (randomSkill <= 20)
        {
            // 60% xác suất cho skill 1 (đánh thường)
            return 1;
        }
        else if (randomSkill <= 40)
        {
            // 30% xác suất cho skill 3
            return 3;
        }
        if (randomSkill <= 70)
        {
            // 30% xác suất cho skill 2
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
                        _CanUseSkillTele = true;
                        Debug.Log(">>>>>>>>>>>>>1");
                        animator.SetBool("IsBossAttack", true);
                        nextSkill1Time = Time.time + skill1Cooldown;
                    }                  
                    break;
                case 2:
                    if (Time.time >= nextSkill2Time)
                    {
                        _CanUseSkillTele = true;
                        Debug.Log(">>>>>>>>>>>>>2");
                        animator.SetBool("IsBossAttack2", true);
                        nextSkill2Time = Time.time + skill2Cooldown;
                    }                   
                    break;
                case 3:
                    if(Time.time >= nextSkill3Time)
                    {
                        Debug.Log(">>>>>>>>>>>>>3");
                        _CanUseSkillTele = true;
                        animator.SetBool("IsBossAttack3", true);
                        nextSkill3Time = Time.time + skill3Cooldown;
                    }
                    break;
                case 4:
                    if (Time.time >= nextSkill4Time)
                    {
                        if (_CanUseSkillTele == true)
                        {
                            StartCoroutine(PrepareToTeleport());
                            nextSkill4Time = Time.time + skill4Cooldown;
                        }
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
        for (int i = 0; i < 30; i++)
        {
            var randomPostion = new Vector2(Random.Range(-7.69f, 7.32f), 6);
            var createMetorite = Instantiate(_Metorite, randomPostion, Quaternion.Euler(0, 0, -90.322f));
            var speedfall = new Vector2(0, -5f);
            createMetorite.GetComponent<Rigidbody2D>().velocity = speedfall;
            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator PrepareToTeleport()
    {
        isPreparingToTeleport = true;
        Debug.Log(">>>>>>>>>4");
        // Tắt hoạt ảnh bị tấn công trong thời gian đệm
        animator.ResetTrigger("IsBossHurt");
        yield return new WaitForSeconds(0.5f); // Thời gian đệm

        // Bắt đầu hoạt ảnh teleport
        animator.SetBool("IsBossAttack", false);
        animator.SetBool("IsBossAttack2", false);
        animator.SetBool("IsBossAttack3", false);
        animator.SetTrigger("IsBossStartTele");

        // Đợi hoạt ảnh teleport kết thúc
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Boss_teleAnimation"))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        yield return new WaitForSeconds(stateInfo.length);

        isPreparingToTeleport = false;
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
        for (int i = 0; i < 30; i++)
        {
            var spawnLazerPosition = new Vector2(Random.Range(-7.3f, 7.8f), 1.6f);
            var createLaze = Instantiate(_Lazer, spawnLazerPosition, Quaternion.Euler(0,0,90));
            createLaze.transform.localScale = new Vector2(18.16f, 8.218f);
            audioSource.PlayOneShot(audioClipLazedSkill);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void TakeDameBoss(int dame)
    {
        if (isPreparingToTeleport) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Boss_TeleStartAnimation") || stateInfo.IsName("Boss_teleAnimation"))
        {
            return;
        }

        if (_HpBossTankValue > 0)
        {
            animator.SetTrigger("IsBossHurt");
            audioSource.PlayOneShot(audioClipHitEnemy);
            _HpBossTankValue -= dame;
            if (_HpBossTankValue <= 0) { _HpBossTankValue = 0; }
            gameController1.StartDameText(dame, _DameText, gameObject.transform);
            hpBossTankSlider.value = _HpBossTankValue;
            _HpText.text = _HpBossTankValue.ToString("");
            animator.SetTrigger("IsBossIdle");
        }
    }
    public void StopAnimationHurt()
    {
        animator.SetTrigger("IsBossIdle");
    }
    public void BossFinishedTele()
    {
        animator.SetTrigger("IsBossFinishedTele");
        // Thực hiện teleport
        if (transform.position.x > 0)
        {
            transform.position = new Vector2(-7.88f, -1.07f);
        }
        else if (transform.position.x < 0)
        {
            transform.position = new Vector2(6.57f, -1.07f);
        }
    }
}
