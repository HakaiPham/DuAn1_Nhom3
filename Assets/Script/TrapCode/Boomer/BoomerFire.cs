using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerFire : MonoBehaviour
{
    // Biến để lưu trữ đối tượng người chơi
    private GameObject Player;

    // Thành phần Rigidbody2D để điều khiển vận tốc của đối tượng
    private Rigidbody2D rb;

    // Lực đẩy của mũi tên
    public float force;

    // Thời gian tồn tại của đối tượng mũi tên
    public float TimerAAF;

    // Vật phẩm NukeBang
    public GameObject NukeBangPrefab;

    void Start()
    {
        // Lấy và lưu trữ thành phần Rigidbody2D của đối tượng
        rb = GetComponent<Rigidbody2D>();

        // Tìm đối tượng người chơi bằng tag "Monster"
        Player = GameObject.FindGameObjectWithTag("Monster");

        // Tính hướng từ mũi tên đến người chơi
        Vector3 direction = Player.transform.position - transform.position;

        // Đặt vận tốc của Rigidbody2D của mũi tên theo hướng hình học và lực đẩy
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        // Tính góc quay của mũi tên để nó nhìn về phía người chơi
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void Update()
    {
        // Tăng thời gian tồn tại của mũi tên
        TimerAAF += Time.deltaTime;

        // Nếu thời gian tồn tại vượt quá 3 giây, hủy đối tượng mũi tên
        if (TimerAAF > 3)
        {
            Destroy(gameObject);
        }
    }

    // Xử lý va chạm với đối tượng khác
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu va chạm với đối tượng có tag "Monster"
        if (other.gameObject.CompareTag("Monster"))
        {
            // Lấy vị trí của đạn khi va chạm
            Vector3 hitPosition = transform.position;

            // Hủy đối tượng mũi tên
            Destroy(gameObject);

            // Bật vật phẩm NukeBang tại vị trí của đạn
            GameObject nukeBang = Instantiate(NukeBangPrefab, hitPosition, Quaternion.identity);

            // Hủy vật phẩm NukeBang sau 2 giây
            Destroy(nukeBang, 2f);
        }
    }
}
