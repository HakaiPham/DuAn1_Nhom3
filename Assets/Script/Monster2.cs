using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        // Lưu trữ scale ban đầu của Boss
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        _AttackBossStart = 0;
        rb=GetComponent<Rigidbody2D>();
        _hpMonsterSummonSlider.maxValue = 100;
        _HpMonsterSummonValue = 100;
        _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString("");
    }
    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position);
        // Tính khoảng cách của nhân vật đến Boss
        Debug.Log("Khoảng cách của player hiện tại là: " + distancePlayer);
        if (distancePlayer > _DetectionRange)
        {
            animator.SetTrigger("IsEnemySummonFly");
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
                    BossSkillAttack();
                }

            }
        }
    }
    public void MoveBoss()
    {
        //animator.SetBool("isBossRun", true);
        // Tính toán hướng tới người chơi nhưng chỉ trên trục X
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);

        // Di chuyển Boss tới vị trí mới
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (distancePlayer > _DetectionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _Flyspeed * Time.deltaTime);
        }
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
    public void BossSkillAttack()
    {
        int randomSkill = Random.Range(0, 3);
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0&&_HpMonsterSummonValue>0)
        {
            switch (randomSkill)
            {
                case 1:
                    animator.SetTrigger("IsEnemySummonSkill1");
                    break;
                case 2:
                    animator.SetTrigger("IsEnemySummonSkill2");
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
        if (_HpMonsterSummonValue <= 0)
        {
            Debug.Log("Enemy Da chet");
            animator.SetTrigger("IsEnemySummonDead");
            animator.ResetTrigger("IsEnemySummonIdle");
            animator.ResetTrigger("IsEnemySummonHurt");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Đã va chạm");
            rb.velocity = Vector3.zero;
        }
    }
    public void EnemySummonHitbyPlayerAttackNormal()
    {
        animator.SetTrigger("IsEnemySummonHurt");
        _HpMonsterSummonValue -= 5;
        _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString();
        _hpMonsterSummonSlider.value = _HpMonsterSummonValue;
        animator.ResetTrigger("IsEnemySummonSkill1");
        animator.ResetTrigger("IsEnemySummonSkill2");
        Invoke("StopHurtEnemyAnimation", 0.4f);
        EnemySummonDead();
    }
    public void EnemySummonHitbyPlayerSkill1()
    {
        _HpMonsterSummonValue -= 10;
        _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString();
        _hpMonsterSummonSlider.value = _HpMonsterSummonValue;
    }
    public void EnemySummonHitbyPlayerSkill2()
    {
        _HpMonsterSummonValue -= 20;
        _HpMonsterSummonText.text = _HpMonsterSummonValue.ToString();
        _hpMonsterSummonSlider.value = _HpMonsterSummonValue;
    }
    public void StartHurtEnemyAnimation()
    {
        animator.SetTrigger("IsEnemySummonHurt");
    }
    public void StopHurtEnemyAnimation()
    {
        animator.SetTrigger("IsEnemySummonIdle");
    }
}
