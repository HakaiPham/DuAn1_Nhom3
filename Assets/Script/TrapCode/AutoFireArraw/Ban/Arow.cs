using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arow : MonoBehaviour
{
    public float speed = 10f;  // Tốc độ của mũi tên
    public float lifetime = 2f;  // Thời gian tồn tại của mũi tên

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Xử lý va chạm với nhân vật
            Destroy(gameObject);
        }
    }
}
