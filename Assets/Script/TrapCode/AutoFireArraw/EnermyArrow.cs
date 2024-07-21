using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyArrow : MonoBehaviour
{
    public GameObject Bullet;       // Prefab của đạn
    public Transform Bulletpos;     // Vị trí khởi tạo đạn
    public float Timer;             // Thời gian cooldown
    public float minRestTime = 1f;  // Thời gian nghỉ ngơi tối thiểu
    public float maxRestTime = 5f;  // Thời gian nghỉ ngơi tối đa
    public float moveSpeed = 2.0f;  // Tốc độ di chuyển trái phải
    public float attackRange = 2.0f; // Phạm vi tấn công trước mặt

    [SerializeField] private float leftBound = -3.0f; // Điểm cuối trái có thể điều chỉnh từ Inspector
    [SerializeField] private float rightBound = 3.0f; // Điểm cuối phải có thể điều chỉnh từ Inspector

    private GameObject FindPlayer;  // Đối tượng nhân vật
    private Animator _Animator;
    private bool inRange;           // Biến để kiểm tra xem nhân vật có trong tầm bắn hay không

    private bool movingRight = true; // Biến để kiểm tra hướng di chuyển
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true; // Biến để theo dõi hướng của quái

    private bool isResting = false;    // Biến để kiểm tra trạng thái nghỉ ngơi

    void Start()
    {
        FindPlayer = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        _Animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Timer += Time.deltaTime; // Tăng thời gian cooldown

        // Tính khoảng cách giữa đối tượng này và nhân vật
        float distance = Vector2.Distance(transform.position, FindPlayer.transform.position);

        // Tính góc giữa đối tượng này và nhân vật trên trục Z
        Vector3 direction = FindPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Kiểm tra hướng di chuyển và cập nhật biến isFacingRight
        isFacingRight = movingRight;

        // Kiểm tra xem nhân vật có trong phạm vi tấn công hay không
        bool inAttackRange = false;
        // Cập nhật vị trí Bulletpos
        UpdateBulletpos();
        if (isFacingRight)
        {
            inAttackRange = distance < attackRange && angle > -15 && angle < 15;
        }
        else
        {
            angle = (angle + 360) % 360; // Điều chỉnh góc để đảm bảo nó luôn dương
            inAttackRange = distance < attackRange && (angle > 165 && angle < 195);
        }

        if (inAttackRange)
        {
            inRange = true;  // Nhân vật đã vào tầm bắn

            // Nếu cooldown đã đủ
            if (Timer > 2)
            {
                Timer = 0; // Đặt lại cooldown
                _Animator.SetTrigger("Fire");
            }
        }
        else
        {
            inRange = false; // Nhân vật không trong tầm bắn
        }

        // Nếu nhân vật không trong tầm bắn, di chuyển trái phải
        if (!inRange && !isResting)
        {
            MoveLeftRight();
            _Animator.SetTrigger("Run"); // Kích hoạt animation Run khi di chuyển
        }
        else
        {
            _Animator.ResetTrigger("Run"); // Reset trigger Run khi trong tầm bắn
        }
    }

    // Phương thức để di chuyển trái phải
    void MoveLeftRight()
    {
        if (movingRight)
        {
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                spriteRenderer.flipX = true; // Quay mặt sang trái
                isFacingRight = false; // Cập nhật hướng quay
                StartCoroutine(Rest()); // Bắt đầu nghỉ ngơi
            }
            else
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                spriteRenderer.flipX = false; // Quay mặt sang phải
                isFacingRight = true; // Cập nhật hướng quay
            }
        }
        else
        {
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                spriteRenderer.flipX = false; // Quay mặt sang phải
                isFacingRight = true; // Cập nhật hướng quay
                StartCoroutine(Rest()); // Bắt đầu nghỉ ngơi
            }
            else
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                spriteRenderer.flipX = true; // Quay mặt sang trái
                isFacingRight = false; // Cập nhật hướng quay
            }
        }
    }

    // Coroutine để xử lý nghỉ ngơi
    IEnumerator Rest()
    {
        isResting = true;

        // Đảm bảo hướng quay đúng khi nghỉ ngơi
        spriteRenderer.flipX = !isFacingRight;
        _Animator.ResetTrigger("Run");
        _Animator.SetTrigger("Ide"); // Kích hoạt animation Ide

        float restTime = Random.Range(minRestTime, maxRestTime); // Thời gian nghỉ ngơi ngẫu nhiên
        yield return new WaitForSeconds(restTime);
        isResting = false;
        _Animator.ResetTrigger("Ide");
    }

    // Phương thức bắn đạn không có tham số
    public void Shoot()
    {
        if (inRange)
        {
            // Xác định hướng bắn dựa trên hướng của Enermy
            Vector3 shootDirection = isFacingRight ? transform.right : -transform.right;

            // Khởi tạo viên đạn đầu tiên tại vị trí Bulletpos
            GameObject bullet1 = Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
            Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();

            // Điều chỉnh tốc độ đạn
            rb1.velocity = shootDirection.normalized * 5f;

            // Xác suất bắn thêm viên đạn
            float chance = Random.value; // Trả về giá trị từ 0.0f đến 1.0f
            if (chance < 0.1f) // 10% xác suất
            {
                StartCoroutine(ShootSecondBullet(shootDirection));
            }
        }
    }

    private IEnumerator ShootSecondBullet(Vector3 shootDirection)
    {
        // Tạm dừng trong một khoảng thời gian ngắn trước khi bắn viên đạn thứ hai
        yield return new WaitForSeconds(0.2f); // Điều chỉnh thời gian nếu cần

        // Khởi tạo viên đạn thứ hai tại vị trí Bulletpos
        GameObject bullet2 = Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
        Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();

        // Lệch hướng của viên đạn thứ hai một chút
        Vector3 bullet2Direction = shootDirection + new Vector3(0.1f, 0.1f, 0f); // Điều chỉnh lệch hướng nếu cần
        rb2.velocity = bullet2Direction.normalized * 5f;
    }


    // Cập nhật vị trí của Bulletpos dựa trên hướng quay của Enermy
    void UpdateBulletpos()
    {
        // Cập nhật Bulletpos dựa trên hướng của Enermy
        Bulletpos.localPosition = isFacingRight ? new Vector3(0.173f, -0.154f, 0f) : new Vector3(-0.173f, -0.154f, 0f);
    }

    void OnDrawGizmos()
    {
        // Vẽ vùng tấn công (hình tròn đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Vẽ hướng bắn cho phạm vi tấn công
        DrawAttackRange();
    }

    // Phương thức để vẽ phạm vi tấn công
    void DrawAttackRange()
    {
        Gizmos.color = Color.yellow;

        // Sử dụng biến isFacingRight để xác định hướng tấn công
        Vector3 facingDirection = isFacingRight ? transform.right : -transform.right;

        // Xác định các góc tấn công dựa trên hướng
        Vector3 directionRight = Quaternion.Euler(0, 0, isFacingRight ? -15 : 15) * facingDirection;
        Vector3 directionLeft = Quaternion.Euler(0, 0, isFacingRight ? 15 : -15) * facingDirection;

        // Vẽ các đường chỉ thị phạm vi tấn công
        Gizmos.DrawLine(transform.position, transform.position + directionRight * attackRange);
        Gizmos.DrawLine(transform.position, transform.position + directionLeft * attackRange);
    }

    // Phương thức để xử lý khi bị tấn công bởi đạn
    public void OnBulletHit()
    {
        StopAllCoroutines(); // Dừng tất cả các coroutine
        isResting = false;     // Đặt trạng thái nghỉ ngơi thành false
        _Animator.ResetTrigger("Ide");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có tag là "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnBulletHit(); // Gọi phương thức xử lý khi bị tấn công bởi đạn
        }
    }
}
