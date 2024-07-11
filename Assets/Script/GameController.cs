using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy; // Kéo đối tượng quái vật vào đây từ Inspector

    void Start()
    {
        // Tìm script EnemyAttack trên đối tượng quái vật và vô hiệu hóa nó
        Monster enemyAttack = enemy.GetComponent<Monster>();
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }
    }
}
