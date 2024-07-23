using System.Collections;
using UnityEngine;

public class SpawnActive : MonoBehaviour
{
    public GameObject spawnPoint; // Điểm tạo (GameObject A)
    public GameObject itemPrefab; // Vật tạo (GameObject)
    public float spawnDelay = 5f; // Thời gian để tạo vật nếu vật tạo bị tắt

    private GameObject spawnedItem; // Biến kiểm tra vật tạo

    private void Start()
    {
        // Bắt đầu Coroutine để kiểm tra liên tục
        StartCoroutine(CheckAndSpawnItem());
    }

    private IEnumerator CheckAndSpawnItem()
    {
        while (true)
        {
            // Kiểm tra nếu vật tạo không tồn tại hoặc bị tắt
            if (spawnedItem == null || !spawnedItem.activeInHierarchy)
            {
                // Nếu vật không tồn tại hoặc bị tắt, tạo vật mới tại điểm tạo
                if (spawnedItem != null)
                {
                    Destroy(spawnedItem); // Xóa vật cũ nếu cần
                }

                spawnedItem = Instantiate(itemPrefab, spawnPoint.transform.position, Quaternion.identity);

                // Đảm bảo vật được kích hoạt
                if (spawnedItem != null)
                {
                    spawnedItem.SetActive(true);
                }
            }

            // Chờ 5 giây trước khi kiểm tra lại
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
