using UnityEngine;

public class Chest_LV5 : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    private Animator _animator;
    private float offset = 0.1f; // Thêm một offset nhỏ để các vật phẩm không chồng lên nhau

    void Start()
    {
        _animator = GetComponent<Animator>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //SpawnItems();
            _animator.SetBool("Open", true);
            Destroy(gameObject, 2f);
        }
    }

    public void SpawnItems()
    {
        int itemCount = 5;

        for (int i = 0; i < itemCount; i++)
        {
            GameObject itemToSpawn = GetRandomItem();
            Vector3 spawnPos = spawnLocation.position + new Vector3(i * offset, 0, 0); // Thêm offset để vật phẩm không chồng lên nhau
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
            Debug.Log("Item spawned: " + itemToSpawn.name); // Log để kiểm tra xem có đủ 5 vật phẩm được tạo ra
        }
    }

    private GameObject GetRandomItem()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.4f)
            return item1;
        else if (randomValue < 0.6f)
            return item2;
        else if (randomValue < 0.75f)
            return item3;
        else if (randomValue < 0.85f)
            return item4;
        else
            return item5;
    }
}
