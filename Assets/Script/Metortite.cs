using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metortite : MonoBehaviour
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
    void Update()
    {
        
    }
    public void DestroyMetorite()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = Vector3.zero;
            _player.TakeDame(30);
            animator.SetBool("IsMetoriteHit", true);
            Invoke("DestroyMetorite", 0.2f);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("IsMetoriteHit", true);
            Invoke("DestroyMetorite", 0.2f);
        }
    }
}
