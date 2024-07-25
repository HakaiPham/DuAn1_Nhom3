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
    Enemy4 _enemy4;
    Enemy5 _enemy5;
    Enemy6 _enemy6;
    Enemy7 _enemy7;
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
        _enemy4 = FindObjectOfType<Enemy4>();
        _enemy5 = FindObjectOfType<Enemy5>();
        _enemy6 = FindObjectOfType<Enemy6>();
        _enemy7 = FindObjectOfType<Enemy7>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyBullet ()
    {
        _Rigidbody.velocity = Vector2.zero;
        _Animator.SetBool("IsBulletAttack", true);
        Destroy(gameObject, 0.4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))         
        {
            _monster =  collision.GetComponent<Monster>();
            _monsterSummon = collision.GetComponent<Monster2>();
            _enemy2 = collision.GetComponent<Enemy2>();
            _enemy3 = collision.GetComponent<Enemy3>();
            _bossTank = collision.GetComponent<BossTank>();
            _enemy4 = collision.GetComponent<Enemy4>();
            _enemy5 = collision.GetComponent<Enemy5>();
            _enemy6 = collision.GetComponent<Enemy6>();
            _enemy7 = collision.GetComponent<Enemy7>();
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
                    DestroyBullet();
                }
                else if (_enemy2 != null && _enemy2.hpEmenyValue > 0)
                {
                    targetMonster = _enemy2.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                if (_enemy3 != null && _enemy3.hpEmenyValue > 0)
                {
                    targetMonster = _enemy3.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                else if (_bossTank != null && _bossTank._HpBossTankValue > 0)
                {
                    targetMonster = _bossTank.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                if (_enemy4 != null && _enemy4.hpEmenyValue > 0)
                {
                    targetMonster = _enemy4.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                else if (_enemy5 != null && _enemy5.hpEmenyValue > 0)
                {
                    targetMonster = _enemy5.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                if (_enemy6 != null && _enemy6.hpEmenyValue > 0)
                {
                    targetMonster = _enemy6.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
                else if (_enemy7 != null && _enemy7.hpEmenyValue > 0)
                {
                    targetMonster = _enemy7.transform;
                    _player.AttackMonsterbySkill2(targetMonster);
                    DestroyBullet();
                }
            }
        }
    }
}
