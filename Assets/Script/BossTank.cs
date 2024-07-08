using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
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
    void Start()
    {
        // Lưu trữ scale ban đầu của Boss
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        _AttackBossStart = 0;
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
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
                animator.SetTrigger("IsBossTankIdle");
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
        //animator.SetBool("isBossRun", true);
        // Tính toán hướng tới người chơi nhưng chỉ trên trục X
        animator.SetBool("IsBossTankRun", true);
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
    public void BossSkill4()
    {
        animator.SetTrigger("IsBossTankSkill4");
    }
    public void BossSkillAttack()
    {
        int randomSkill = Random.Range(1, 4);
        _AttackBossStart -= Time.deltaTime;
        if (_AttackBossStart <= 0)
        {
            switch (randomSkill)
            {
                case 1:
                    animator.SetTrigger("IsBossTankSkill1");
                    break;
                case 2:                   
                    animator.SetTrigger("IsBossTankSkill2");                 
                    break;
                case 3:
                    animator.SetTrigger("IsBossTankSkill3");
                    break;
            }
            _AttackBossStart = _AttackBossCoolDown;
        }
        else if (_AttackBossStart > 0) //Nếu còn đang trong thời gian cooldown
        {
            animator.SetTrigger("IsBossTankIdle");
            animator.SetBool("IsBossTankRun", false);
        }
    }
}
