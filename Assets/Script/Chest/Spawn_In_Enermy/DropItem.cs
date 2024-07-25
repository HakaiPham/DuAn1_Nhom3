using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject itemPrefab; // Vật phẩm sẽ được tạo ra
    private Vector3 lastPosition; // Vị trí của kẻ thù

    void Start()
    {
        // Lưu vị trí hiện tại của kẻ thù vào biến lastPosition
        lastPosition = transform.position;
    }

    void Update()
    {
        // Không cần kiểm tra trong Update() nếu không có điều kiện đặc biệt nào
    }

    // Gọi phương thức này từ nơi nào đó khi kẻ thù bị tiêu diệt
    public void OnEnemyDestroyed()
    {
        // Tạo vật phẩm tại vị trí của đối tượng này (kẻ thù)
        Instantiate(itemPrefab, lastPosition, Quaternion.identity);

        // Tùy chọn: hủy đối tượng này sau khi tạo vật phẩm
        // Destroy(this.gameObject);
    }

    // Được gọi khi kẻ thù bị tiêu diệt hoặc bị phá hủy
    void OnDestroy()
    {
        // Gọi phương thức OnEnemyDestroyed() khi đối tượng bị hủy
        OnEnemyDestroyed();
    }
}
