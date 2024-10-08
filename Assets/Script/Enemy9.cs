﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy9 : MonoBehaviour
{
    // Start is called before the first frame update
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
    public Slider _EnemyHp;
    public TextMeshProUGUI _HpEnemyText;
    public int hpEmenyValue;
    Rigidbody2D _rigidbody2;
    bool _IsDead;
    [SerializeField] private GameObject _HpEnemyOff;
    public TextMeshProUGUI _DameText;
    GameController1 gameController1;
    AudioSource audioSource;
    public AudioClip audioClipHitEnemy;
    public AudioClip audioClipDead;
    CircleCollider2D circleCollider2D;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _IsAttacking = false;
        _EnemyAttackTimeStart = 0;
        _playerHp = FindObjectOfType<Player>();
        _EnemyHp.maxValue = 50;
        hpEmenyValue = 50;
        _HpEnemyText.text = hpEmenyValue.ToString("");
        _rigidbody2 = GetComponent<Rigidbody2D>();
        _IsDead = false;
        gameController1 = FindObjectOfType<GameController1>();
        audioSource = GetComponent<AudioSource>();
        circleCollider2D = GetComponent<CircleCollider2D>();
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
        if (_IsAttacking == true || _playerHp.hpValue <= 0) return;
        else
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
            _HpEnemyText.transform.localScale = new Vector3(-1, 1, 1);
            if (isMoveLeftOrRight == false)
            {
                move = Vector2.left;
                _HpEnemyText.transform.localScale = new Vector3(1, 1, 1);
            }

            var enemyScale = transform.localScale;
            if (enemyScale.x > 0 && isMoveLeftOrRight == true ||
                enemyScale.x < 0 && isMoveLeftOrRight == false)
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
            _animator.SetBool("IsEnemy9Run", true);
            _animator.SetBool("IsEnemy9Attack", false);
            transform.Translate(move * _enemyMoveSpeed * Time.deltaTime);
        }
    }
    public void SkillMonster()
    {
        if (_playerHp.hpValue > 0 && _IsAttacking == true && _IsDead == false)
        {
            Vector3 scale = transform.localScale;
            if (_player.position.x < transform.position.x && scale.x < 0
                || _player.position.x > transform.position.x && scale.x > 0)
            {
                scale.x *= -1;
                transform.localScale = scale;
                _HpEnemyText.transform.localScale = new Vector3(scale.x, 1, 1);
            }
            _animator.SetBool("IsEnemy9Attack", true);
        }
        else
        {
            if (_EnemyAttackTimeStart > 0)
            {
                _EnemyAttackTimeStart -= Time.deltaTime;
                _animator.SetBool("IsEnemy9Attack", false);
            }
        }
        if (_playerHp.hpValue <= 0)
        {
            _animator.SetBool("IsEnemy9Run", true);
            _animator.SetBool("IsEnemy9Attack", false);
        }
    }
    public void Hpnv()
    {
        if (_playerHp.hpValue > 0)
        {
            _playerHp.TakeDame(5);
        }
        else
        {
            return;
        }
    }
    public void StopHitEnemy()
    {
        _animator.SetTrigger("IsEnemy9Idle");
    }
    public void EnemyDead()
    {
        if (hpEmenyValue <= 0 && _IsDead == false)
        {
            _animator.SetBool("IsEnemy9Run", false);
            _animator.SetBool("IsEnemy9Attack", false);
            _animator.ResetTrigger("IsEnemy9Idle");
            _animator.ResetTrigger("IsEnemy9Hurt");
            _IsDead = true;
            _HpEnemyOff.SetActive(false);
            _rigidbody2.velocity = Vector2.zero;
            audioSource.PlayOneShot(audioClipDead);
            _animator.SetTrigger("IsEnemy9Dead");
            Destroy(gameObject, 2f);
        }
    }
    public void Enemy9TakeDame(int dame)
    {
        if (hpEmenyValue > 0)
        {
            AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            bool canAttack = true;
            if (animatorStateInfo.IsName("Enemy9_AttackAnimation") && canAttack == true) return;
            if (hpEmenyValue > 0)
            {
                _animator.SetTrigger("IsEnemy9Hurt");
                audioSource.PlayOneShot(audioClipHitEnemy);
                hpEmenyValue -= dame;
                if (hpEmenyValue <= 0) { hpEmenyValue = 0; }
                gameController1.StartDameText(dame, _DameText, gameObject.transform);
                _EnemyHp.value = hpEmenyValue;
                _HpEnemyText.text = hpEmenyValue.ToString("");
                Invoke("StopHitEnemy", 0.33f);
                canAttack = false;
            }
        }
    }
}
