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
    Monster2 _monsterSummon;
    Enemy2 _enemy2;
    Enemy3 _enemy3;
    BossTank _bossTank;
    void Start()
    {
        _Animator = GetComponent<Animator>();
        _CircleCollider = GetComponent<CircleCollider2D>();
        _Rigidbody = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
        targetMonster = null;
        _monster = FindObjectOfType<Monster>();
        _monsterSummon = FindObjectOfType<Monster2>();
        _enemy2 = FindObjectOfType<Enemy2>();
        _enemy3 = FindObjectOfType<Enemy3>();
        _bossTank = FindObjectOfType<BossTank>();
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
        if (collision.gameObject.CompareTag("Monster")|| 
            collision.gameObject.CompareTag("EnemySummon")|| 
            collision.gameObject.CompareTag("BossTank"))
        {
            _monster =  collision.GetComponent<Monster>();
            _monsterSummon = collision.GetComponent<Monster2>();
            _enemy2 = collision.GetComponent<Enemy2>();
            _enemy3 = collision.GetComponent<Enemy3>();
            _bossTank = collision.GetComponent<BossTank>();
            if (_monster != null&&_monster.hpEmenyValue>0)
            {
                targetMonster = _monster.transform;
                Debug.Log( "vị trị hiện tại của quái là: " + targetMonster);
                _player.AttackMonsterbySkill2(targetMonster);
                _Rigidbody.velocity = Vector2.zero;
                _Animator.SetBool("IsBulletAttack", true);
                Invoke("DestroyBullet", 0.4f);
            }
            else
            {
                if (_monsterSummon != null&& _monsterSummon._HpMonsterSummonValue > 0)
                {
                    Debug.Log("vị trị hiện tại của quái là: " + targetMonster);
                    targetMonster = _monsterSummon.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    _Rigidbody.velocity = Vector2.zero;
                    _Animator.SetBool("IsBulletAttack", true);
                    Invoke("DestroyBullet", 0.4f);
                }
                else if (_enemy2 != null && _enemy2.hpEmenyValue > 0)
                {
                    targetMonster = _enemy2.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    _Rigidbody.velocity = Vector2.zero;
                    _Animator.SetBool("IsBulletAttack", true);
                    Invoke("DestroyBullet", 0.4f);
                }
                if (_enemy3 != null && _enemy3.hpEmenyValue > 0)
                {
                    targetMonster = _enemy3.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    _Rigidbody.velocity = Vector2.zero;
                    _Animator.SetBool("IsBulletAttack", true);
                    Destroy(gameObject,0.4f);
                }
                else if (_bossTank != null && _bossTank._HpBossTankValue > 0)
                {
                    targetMonster = _bossTank.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    _Rigidbody.velocity = Vector2.zero;
                    _Animator.SetBool("IsBulletAttack", true);
                    Invoke("DestroyBullet", 0.4f);
                }
            }
        }
    }
}
