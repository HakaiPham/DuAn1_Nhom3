using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : MonoBehaviour
{
  private CapsuleCollider2D _SpearTrapCollider2d;
    Player _Player;
    void Start()
    {
        // Gán CapsuleCollider2D của SpearTrap khi bắt đầu
        _SpearTrapCollider2d = GetComponent<CapsuleCollider2D>();
        _Player = FindObjectOfType<Player>();
    }
    public void DisableColider()
    {
        _SpearTrapCollider2d.enabled = false;
    }

    public void EnableColider()
    {
        _SpearTrapCollider2d.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _Player.TakeDame(5);
        }
    }

}
