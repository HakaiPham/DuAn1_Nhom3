using System.Collections;
using UnityEngine;

public class SpearSpawn : MonoBehaviour
{
    public GameObject spawnPoint; // Điểm tạo
    public GameObject activationPoint; // Điểm kích hoạt
    public GameObject spearPrefab; // Vật cần tạo (Spear)
    public float distanceBetweenSpears = 0.1f; // Khoảng cách giữa các Spear
    public float destroyAfterSeconds = 3f; // Thời gian để hủy các Spear

    private bool isSpawning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem collider chạm vào có phải là Player không
        if (other.CompareTag("Player"))
        {
            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnSpears());
               
            }
        }
    }

    private IEnumerator SpawnSpears()
    {
        // Tạo 10 Spear
        for (int i = 0; i < 10; i++)
        {
            Instantiate(spearPrefab, spawnPoint.transform.position + new Vector3(i * distanceBetweenSpears, 0, 0), Quaternion.identity);
        }

        // Hủy toàn bộ Spear sau thời gian quy định
        yield return new WaitForSeconds(destroyAfterSeconds);

        GameObject[] spears = GameObject.FindGameObjectsWithTag("Spear");
        foreach (GameObject spear in spears)
        {
            Destroy(spear);
        }

        // Tắt hoàn toàn điểm kích hoạt
        activationPoint.SetActive(false);

        // Cập nhật trạng thái spawning
        isSpawning = false;
    }
}
