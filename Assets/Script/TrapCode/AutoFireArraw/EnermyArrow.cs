using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnermyArrow : MonoBehaviour
{
    public GameObject Bullet;       // Prefab của đạn
    public Transform Bulletpos;     // Vị trí khởi tạo đạn
    public float Timer;             // Thời gian cooldown
    private GameObject FindPlayer;  // Đối tượng nhân vật
    private Animator _Animator;
    private bool inRange;           // Biến để kiểm tra xem nhân vật có trong tầm bắn hay không

    public float moveSpeed = 2.0f;  // Tốc độ di chuyển trái phải
    public float moveRange = 3.0f;  // Khoảng cách di chuyển trái phải
    private float startPos;         // Vị trí ban đầu để tính khoảng cách di chuyển
    private bool movingRight = true;// Biến để kiểm tra hướng di chuyển
    public float distance;

    void Start()
    {
        FindPlayer = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        _Animator = GetComponent<Animator>();
        startPos = transform.position.x;
    }

    void Update()
    {
        Timer += Time.deltaTime; // Tăng thời gian cooldown

        // Tính khoảng cách giữa đối tượng này và nhân vật
        distance = Vector2.Distance(transform.position, FindPlayer.transform.position);
        Debug.Log(distance);

        // Tính góc giữa đối tượng này và nhân vật trên trục Z
        Vector3 direction = FindPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Kiểm tra xem góc có nằm trong khoảng cho phép không (-45 độ phía dưới)
        if (distance < 2 && (angle > -45 && angle < 45))
        {
            inRange = true;  // Đánh dấu nhân vật đã vào tầm bắn

            // Nếu cooldown đã đủ
            if (Timer > 2)
            {
                Timer = 0; // Đặt lại cooldown
                Shoot();   // Bắn đạn và kích hoạt animation Fire 
                _Animator.SetTrigger("Fire");
            }
        }
        else
        {
            inRange = false; // Đánh dấu nhân vật không trong tầm bắn
        }

        // Nếu nhân vật không trong tầm bắn, di chuyển trái phải
        if (!inRange)
        {
            MoveLeftRight();
            // Reset trigger Fire khi không trong tầm bắn
            _Animator.SetTrigger("Run");
        }
        else
        {
            _Animator.ResetTrigger("Run");
        }
    }

    // Phương thức để di chuyển trái phải
    void MoveLeftRight()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (transform.position.x >= startPos + moveRange)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (transform.position.x <= startPos - moveRange)
            {
                movingRight = true;
            }
        }
    }

    // Phương thức để bắn đạn
    public void Shoot()
    {
        // Chỉ bắn đạn nếu nhân vật trong tầm bắn
        if (inRange)
        {
            // Khởi tạo đạn tại vị trí Bulletpos
            Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
            // Kích hoạt animation Fire
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.Log("OnDrawGizmosSelected called");
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

}
