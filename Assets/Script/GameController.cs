using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D _boss1Rb;
    public Monster2 _ScriptMonster2;
    void Start()
    {
        // Tìm script EnemyAttack trên đối tượng quái vật và vô hiệu hóa nó
        _ScriptMonster2.enabled = false;
    }
    private void Update()
    {
        _boss1Rb.velocity = Vector2.down * 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("đã va chạm với ground");
            _boss1Rb.velocity = Vector2.zero;
            _ScriptMonster2.enabled = true;
        }
    }
}
