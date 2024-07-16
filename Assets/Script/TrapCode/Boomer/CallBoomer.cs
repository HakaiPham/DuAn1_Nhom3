using UnityEngine;

public class CallBoomer : MonoBehaviour
{
    // Tham chiếu đến Prefab của vật phẩm
    public GameObject itemPrefab;

    // Tham chiếu đến GameObject rỗng (CreateEmpty)
    public Transform createEmptyTransform;

    // Số lần tối đa có thể tạo vật phẩm
    private int maxSpawnCount = 2;

    // Số lần đã tạo vật phẩm
    private int currentSpawnCount = 0;

    // Thời gian hồi chiêu (giây)
    private float cooldownTime = 10f;

    // Thời gian còn lại của hồi chiêu
    private float cooldownTimer = 0f;

    void Update()
    {
        // Giảm thời gian hồi chiêu theo thời gian
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Kiểm tra khi phím N được nhấn, số lần tạo chưa đạt tối đa và hồi chiêu đã hết
        if (Input.GetKeyDown(KeyCode.N) && currentSpawnCount < maxSpawnCount && cooldownTimer <= 0f)
        {
            // Tạo vật phẩm tại vị trí của CreateEmpty
            Instantiate(itemPrefab, createEmptyTransform.position, createEmptyTransform.rotation);

            // Tăng số lần đã tạo vật phẩm
            currentSpawnCount++;

            // Đặt lại thời gian hồi chiêu
            cooldownTimer = cooldownTime;
        }
    }
}
