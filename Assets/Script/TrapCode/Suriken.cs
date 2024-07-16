using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suriken : MonoBehaviour
{
    public float speed = 5f;  // Tốc độ di chuyển của suriken
    public float leftBoundary = -5f;  // Giới hạn bên trái
    public float rightBoundary = 5f;  // Giới hạn bên phải

    private bool movingRight = true;

    void Update()
    {
        // Kiểm tra hướng di chuyển và cập nhật vị trí
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                movingRight = true;
            }
        }
    }
}
