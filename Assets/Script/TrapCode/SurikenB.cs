using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurikenB : MonoBehaviour
{
    public float speed = 5f;  // Tốc độ di chuyển của SurikenB
    public float topBoundary = 5f;  // Giới hạn trên
    public float bottomBoundary = -5f;  // Giới hạn dưới

    private bool movingUp = true;

    void Update()
    {
        // Kiểm tra hướng di chuyển và cập nhật vị trí
        if (movingUp)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (transform.position.y >= topBoundary)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if (transform.position.y <= bottomBoundary)
            {
                movingUp = true;
            }
        }
    }
}
