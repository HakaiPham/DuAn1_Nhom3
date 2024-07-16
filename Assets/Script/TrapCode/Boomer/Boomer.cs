using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MonoBehaviour
{
    // Tốc độ di chuyển
    public float speed = 5f;

    // Điểm bắt đầu và kết thúc
    public Vector3 startPoint;
    public Vector3 endPoint;

    // Prefab của đạn
    public GameObject Bullet;
    // Vị trí khởi tạo đạn
    public Transform Bulletpos;
    // Phạm vi tấn công theo chiều ngang
    public float attackRangeX = 7f;
    // Phạm vi tấn công theo chiều dọc (chỉ phía dưới)
    public float attackRangeY = 7f;

    // Thời gian cooldown
    private float Timer;
    // Đối tượng Monster
    private GameObject FindPlayer;
    // Biến để kiểm tra xem Monster có trong tầm bắn hay không
    private bool inRange;

    void Start()
    {
        // Đặt vị trí ban đầu của đối tượng
        transform.position = startPoint;
        FindPlayer = GameObject.FindGameObjectWithTag("Monster");
        inRange = false;
    }

    void Update()
    {
        MoveBoomer();
        CheckAndShoot();
    }

    // Hàm để di chuyển đối tượng từ trái sang phải
    void MoveBoomer()
    {
        // Di chuyển đối tượng về phía vị trí kết thúc
        transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);

        // Kiểm tra nếu đối tượng đã đến vị trí kết thúc
        if (transform.position == endPoint)
        {
            // Tắt vật thể
            gameObject.SetActive(false);
        }
    }

    // Hàm để kiểm tra và bắn đạn
    void CheckAndShoot()
    {
        if (FindPlayer == null)
            return;

        // Tính khoảng cách theo chiều ngang giữa đối tượng này và Monster
        float distanceX = Vector2.Distance(transform.position, FindPlayer.transform.position);
        // Tính khoảng cách theo chiều dọc giữa đối tượng này và Monster (chỉ phía dưới)
        float distanceY = FindPlayer.transform.position.y - transform.position.y;

        // Nếu khoảng cách nằm trong phạm vi tấn công theo chiều ngang và chiều dọc (phía dưới) và chưa bắn
        if (distanceX <= attackRangeX && distanceY <= attackRangeY && !inRange)
        {
            inRange = true;  // Đánh dấu Monster đã vào tầm bắn
            Shoot();         // Bắn đạn một lần duy nhất
        }
    }

    // Phương thức để bắn đạn
    public void Shoot()
    {
        // Khởi tạo đạn tại vị trí Bulletpos
        Instantiate(Bullet, Bulletpos.position, Quaternion.identity);
    }
}
