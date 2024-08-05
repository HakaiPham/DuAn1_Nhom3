using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBoss : MonoBehaviour
{
    Player player;
    BoxCollider2D boxCollider;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LazerHitPLayer()
    {
        if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Player"))){
            player.TakeDame(10);
        }
        animator.SetBool("IsBossLazer", true);
    }
    public void DestroyLazer()
    {
        Destroy(gameObject);
    }
}
