using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap1 : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    Player player;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("IsTrap1Start", true);
            player.TakeDame(10);
            Destroy(gameObject,0.5f);
        }
    }
}
