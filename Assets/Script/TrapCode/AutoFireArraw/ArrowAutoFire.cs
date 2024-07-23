using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAutoFire : MonoBehaviour
{
    public GameObject Bullet;       // Prefab của đạn
    public Transform Bulletpos;     // Vị trí khởi tạo đạn
    public float cooldown = 2f;     // Thời gian cooldown
    public float fireRange = 1f;    // Phạm vi bắn (có thể thay đổi từ Inspector)
    public float shootAngle = 15f;  // Góc bắn tối đa (có thể thay đổi từ Inspector)

    private GameObject FindPlayer;  // Đối tượng nhân vật
    public GameObject rotate;       // GameObject của súng để xoay
    private Animator _Animator;
    private bool inRange;           // Biến để kiểm tra xem nhân vật có trong tầm bắn hay không

    private float timer;            // Thời gian cooldown

    void Start()
    {
        FindPlayer = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        _Animator = GetComponent<Animator>();
        timer = cooldown;  // Đặt timer ban đầu
    }

    void Update()
    {
        timer += Time.deltaTime; // Tăng thời gian cooldown

        // Tính khoảng cách giữa đối tượng này và nhân vật
        float distance = Vector2.Distance(transform.position, FindPlayer.transform.position);

        // Kiểm tra xem nhân vật có trong tầm bắn không
        if (distance < fireRange)
        {
            inRange = true;  // Đánh dấu nhân vật đã vào tầm bắn
        }
        else
        {
            inRange = false; // Đánh dấu nhân vật không trong tầm bắn
        }

        // Xoay súng về phía nhân vật và kích hoạt animation FIRE
        if (rotate != null && FindPlayer != null && inRange)
        {
            // Tính hướng từ súng đến nhân vật
            Vector3 direction = FindPlayer.transform.position - rotate.transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

            // Áp dụng xoay cho súng
            rotate.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Kích hoạt animation FIRE
            _Animator.SetBool("FIRE", true);

            // Kiểm tra cooldown để bắn đạn
            if (timer >= cooldown)
            {
                timer = 0; // Đặt lại cooldown
               // Shoot();   // Bắn đạn
            }
        }
        else
        {
            // Nếu không có nhân vật hoặc súng, vô hiệu hóa animation FIRE
            _Animator.SetBool("FIRE", false);
        }
    }

    // Phương thức để bắn đạn, sẽ được gọi bởi sự kiện animation
    public void Shoot()
    {
        if (inRange) // Kiểm tra lại xem nhân vật có trong tầm bắn không
        {
            // Khởi tạo đạn tại vị trí Bulletpos
            Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        // Vẽ phạm vi bắn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange); // Vẽ phạm vi bắn dưới dạng hình cầu

        // Vẽ phạm vi góc bắn
        if (rotate != null)
        {
            Gizmos.color = Color.green;
            Vector3 rightLimit = Quaternion.Euler(0, 0, shootAngle) * rotate.transform.right * fireRange;
            Vector3 leftLimit = Quaternion.Euler(0, 0, -shootAngle) * rotate.transform.right * fireRange;

            Gizmos.DrawLine(transform.position, transform.position + rightLimit);
            Gizmos.DrawLine(transform.position, transform.position + leftLimit);
        }
    }
}
