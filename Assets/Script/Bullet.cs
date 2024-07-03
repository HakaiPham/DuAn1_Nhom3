using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Animator _Animator;
    CircleCollider2D _CircleCollider;
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CircleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HitMonster();

    }
    public void HitMonster()
    {
        if (_CircleCollider.IsTouchingLayers(LayerMask.GetMask("Monster")))
        {
            _Animator.SetBool("IsBulletAttack", true);
        }
    }
    public void DestroyBullet()
    {
        if (_CircleCollider.IsTouchingLayers(LayerMask.GetMask("Monster")))
        {
            Destroy(gameObject);
        }
    }
}
