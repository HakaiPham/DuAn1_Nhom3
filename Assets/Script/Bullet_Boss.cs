using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Rigidbody2D rb;
    Player _player;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            _player.TakeDame(5);
            animator.SetBool("IsBulletBosshit", true);
            Invoke("DestroyBullet", 0.3f);
        }
    }
}
