using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAutoFire : MonoBehaviour
{
    public GameObject Bullet;       // Prefab của đạn
    public Transform Bulletpos;     // Vị trí khởi tạo đạn
    public float Timer;             // Thời gian cooldown
    private GameObject FindPlayer;  // Đối tượng nhân vật
    public GameObject rotate;       // GameObject của súng để xoay
    private Animator _Animator;
    private bool inRange;           // Biến để kiểm tra xem nhân vật có trong tầm bắn hay không

    void Start()
    {
        FindPlayer = GameObject.FindGameObjectWithTag("Player");
        inRange = false;
        _Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Timer += Time.deltaTime; // Tăng thời gian cooldown

        // Tính khoảng cách giữa đối tượng này và nhân vật
        float distance = Vector2.Distance(transform.position, FindPlayer.transform.position);
        

        // Nếu khoảng cách nhỏ hơn 2 đơn vị  
        if (distance < 0.5)
        {
            inRange = true;  // Đánh dấu nhân vật đã vào tầm bắn
            Timer += Time.deltaTime;
            // Nếu cooldown đã đủ
            if (Timer > 2)
            {
                Timer = 0; // Đặt lại cooldown
                Shoot();   // Bắn đạn
            }
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
            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            // Áp dụng xoay cho súng
            rotate.transform.rotation = Quaternion.Euler(0, 0, rot - 90);

            // Kích hoạt animation FIRE
            _Animator.SetBool("FIRE", true);
        }
        else
        {
            // Nếu không có nhân vật hoặc súng, vô hiệu hóa animation FIRE
            _Animator.SetBool("FIRE", false);
        }
    }

    // Phương thức để bắn đạn
    public void Shoot()
    {
        // Khởi tạo đạn tại vị trí Bulletpos
        Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
    }
}
