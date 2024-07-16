using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Animator _Animator;
    CircleCollider2D _CircleCollider;
    Rigidbody2D _Rigidbody;
    Player _player;
    Transform targetMonster;
    Monster _monster;
    Monster2 _monster2;
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CircleCollider = GetComponent<CircleCollider2D>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
        targetMonster = null;
        _monster = FindObjectOfType<Monster>();
        _monster2 = FindObjectOfType<Monster2>();
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
        if (collision.gameObject.CompareTag("Monster")|| collision.gameObject.CompareTag("EnemySummon"))
        {
            Monster currentMonster = collision.GetComponent<Monster>();
            Monster2 currentMonsterSummon = collision.GetComponent<Monster2>();
            if (currentMonster != null&&_monster.hpEmenyValue>0)
            {
                targetMonster = currentMonster.transform;
                Debug.Log( "vị trị hiện tại của quái là: " + targetMonster);
                _player.AttackMonsterbySkill2(targetMonster);
                _Rigidbody.velocity = Vector2.zero;
                _Animator.SetBool("IsBulletAttack", true);
                Invoke("DestroyBullet", 0.4f);
            }
            else
            {
                if (currentMonsterSummon != null&& _monster2._HpMonsterSummonValue > 0)
                {
                    Debug.Log("vị trị hiện tại của quái là: " + targetMonster);
                    targetMonster = currentMonsterSummon.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    _Rigidbody.velocity = Vector2.zero;
                    _Animator.SetBool("IsBulletAttack", true);
                    Invoke("DestroyBullet", 0.4f);
                }
            }
        }
    }
}
