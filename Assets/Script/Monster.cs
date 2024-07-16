using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    [SerializeField] private float _enemyMoveSpeed;
    [SerializeField] private float _Right;
    [SerializeField] private float _Left;
    [SerializeField] private float _AttackRange;
    public Transform _player;
    private Animator _animator;
    private bool isMoveLeftOrRight;
    private bool _playerInRange;
    private bool _IsAttacking;
    float _EnemyAttackTimeStart;
    [SerializeField] private float _EnemyAttackTime;
    Player _playerHp;
    public  Slider _EnemyHp;
    public TextMeshProUGUI _HpEnemyText;
    public int hpEmenyValue;
    Rigidbody2D _rigidbody2;
    bool _IsDead;
    [SerializeField] private GameObject _HpEnemyOff;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _IsAttacking = false;
        //_EnemyAttackTimeStart = 0;
        _playerHp = FindObjectOfType<Player>();
        _EnemyHp.maxValue = 50;
        hpEmenyValue = 50;
        _HpEnemyText.text = hpEmenyValue.ToString("");
        _rigidbody2 = GetComponent<Rigidbody2D>();
        _IsDead = false;
    }

    void Update()
    {
        var distancePlayer = Vector2.Distance(transform.position, _player.position);

        // Kiểm tra xem người chơi có nằm trong phạm vi tấn công không
        if (distancePlayer <= _AttackRange)
        {
            _IsAttacking = true;
        }
        else
        {
            _IsAttacking = false;
        }

        //Nếu người chơi nằm trong phạm vi tấn công, quái sẽ dừng lại và tấn công

        if (_IsAttacking == false)
        {
            EnemyMove();
        }
        //StopAttack();
        EnemyDead();
    }
    public void EnemyMove()
    {
        var enemyPosition = transform.localPosition;

        if (enemyPosition.x >= _Right)
        {
            isMoveLeftOrRight = false;
        }
        else if (enemyPosition.x <= _Left)
        {
            isMoveLeftOrRight = true;
        }

        var move = Vector2.right;
        _HpEnemyText.transform.localScale = new Vector3(1, 1, 1);
        if (isMoveLeftOrRight == false)
        {
            move = Vector2.left;
            _HpEnemyText.transform.localScale = new Vector3(-1, 1, 1);
        }

        var enemyScale = transform.localScale;
        if (enemyScale.x > 0 && isMoveLeftOrRight == false || enemyScale.x < 0 && isMoveLeftOrRight == true)
        {
            enemyScale.x *= -1;
            transform.localScale = enemyScale;
        }
        else
        {
            if (hpEmenyValue <= 0)
            {
                return;
            }
        }
        _animator.SetBool("IsEnemyRun", true);
        _animator.SetBool("IsEnemyAttack1", false);
        _animator.SetBool("IsEnemyAttack2", false);
        transform.Translate(move * _enemyMoveSpeed * Time.deltaTime);
    }
    public void SkillMonster()
    {
        if (_playerHp.hpValue > 0)
        {
            int randomSkill = Random.Range(1, 3);
            _EnemyAttackTimeStart -= Time.deltaTime;
            if (_EnemyAttackTimeStart <= 0)
            {
                switch (randomSkill)
                {
                    case 1:
                        _animator.SetBool("IsEnemyAttack1", true);
                        if (_IsAttacking == true && _playerHp.hpValue > 0)
                        {
                            //Debug.Log("Người chơi trong phạm vi tấn công nên mất máu");
                            //Debug.Log("Skill hiện tại là: " + randomSkill);
                            Invoke("Hpnv", 0.51f);
                            _playerHp.HpPlayer();
                        }
                        break;
                    case 2:
                        _animator.SetBool("IsEnemyAttack2", true);
                        if (_IsAttacking == true && _playerHp.hpValue > 0)
                        {
                            //Debug.Log("Người chơi trong phạm vi tấn công nên mất máu");
                            //Debug.Log("Skill hiện tại là: " + randomSkill);
                            Invoke("Hpnv", 0.51f);
                            _playerHp.HpPlayer();
                        }
                    ; break;
                }
                _EnemyAttackTime = _EnemyAttackTimeStart;
            }
        
        }
        else
        {
            if (_EnemyAttackTimeStart > 0)
            {
                _EnemyAttackTimeStart -= Time.deltaTime;
                _animator.SetBool("IsEnemyAttack1", false);
                _animator.SetBool("IsEnemyAttack2", false);
            }
            else if (_playerHp.hpValue <= 0)
            {
                _animator.SetBool("IsEnemyRun", true);
                _animator.SetBool("IsEnemyAttack1", false);
                _animator.SetBool("IsEnemyAttack2", false);
                _playerHp.Dead();
            }
        }
    }
    public void Hpnv()
    {
        _playerHp.HurtInRangeAttackMonster();
    }
    public void HitEnemy()
    {
        _animator.SetTrigger("IsEnemyHurt");
    }
    public void StopHitEnemy()
    {
        _animator.SetTrigger("IsEnemyIdle");
    }
    public void EnemyDead()
    {
        if (hpEmenyValue <= 0&&_IsDead==false)
        {
            _IsDead = true;
            _HpEnemyOff.SetActive(false);
            _rigidbody2.velocity = Vector2.zero;
            _animator.SetBool("IsEnemyRun", false);
            _animator.SetTrigger("IsEnemyDead");
        }
    }
}
