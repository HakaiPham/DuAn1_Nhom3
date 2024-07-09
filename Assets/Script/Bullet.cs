using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Animator _Animator;
    CircleCollider2D _CircleCollider;
    Rigidbody2D _Rigidbody;
    Monster _HpMonster;
    Player _player;
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CircleCollider = GetComponent<CircleCollider2D>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _HpMonster = FindObjectOfType<Monster>();
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyBullet ()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (_HpMonster.hpEmenyValue > 0)
            {
                _player.AttackMonsterbySkill2();
                _Rigidbody.velocity = Vector2.zero;
                _Animator.SetBool("IsBulletAttack", true);
                Invoke("DestroyBullet", 0.4f);
            }
        }
    }
}
