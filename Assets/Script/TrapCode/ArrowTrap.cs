using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    public GameObject arrowPrefab;  // Prefab của mũi tên
    public Transform firePoint;  // Vị trí bắn mũi tên
    public float fireRate = 0.5f;  // Tốc độ bắn (số lần bắn mỗi giây)
    public float arrowSpeed = 10f;  // Tốc độ của mũi tên
    public float arrowLifetime = 2f;  // Thời gian tồn tại của mũi tên

    private float nextFireTime = 0f;

    void Update()
    {
        ShootArrowContinuously();
    }

    void ShootArrowContinuously()
    {
        if (Time.time >= nextFireTime)
        {
            ShootArrow();
            nextFireTime = Time.time + 2f;  // Bắn mỗi 2 giây
        }
    }

    void ShootArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * arrowSpeed;
        }
        Destroy(arrow, arrowLifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Xử lý va chạm với nhân vật
            Destroy(other.gameObject);
        }
    }
}
