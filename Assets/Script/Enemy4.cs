using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy4 : MonoBehaviour
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
    BoxCollider2D _boxconllider2D;
    CircleCollider2D _circleCollider2D;
    bool _isEnemyStartIntro;
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
        _boxconllider2D = GetComponent<BoxCollider2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _isEnemyStartIntro = false;
        isMoveLeftOrRight = true;
    }
    void Update()
    {
        var distancePlayer = Vector2.Distance(transform.position, _player.position);

        // Kiểm tra xem người chơi có nằm trong phạm vi tấn công không
        if (distancePlayer <= _AttackRange)
        {
            if (_isEnemyStartIntro == false)
            {
                _isEnemyStartIntro = true;
                _animator.SetTrigger("IsEnemy4Intro");
            }
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
    public void StopIntro()
    {
        _animator.ResetTrigger("IsEnemy4Intro");
        _animator.SetTrigger("IsEnemy4Idle");
    }
    public void EnemyMove()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Enemy4_IntroAnimation") ||
            stateInfo.IsName("Enemy4_Intro2Animation")) 
        {
            _HpEnemyOff.SetActive(false);
            return;
        }
        if (_IsAttacking == true) return;
        else
        {
            if (hpEmenyValue > 0)
            {
                _HpEnemyOff.SetActive(true);
                _boxconllider2D.enabled = true;
                _circleCollider2D.enabled = false;
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
                _animator.SetBool("IsEnemy4Run", true);
                _animator.SetBool("IsEnemy4Attack", false);
                transform.Translate(move * _enemyMoveSpeed * Time.deltaTime);
            }        
        }
    }
    public void SkillMonster()
    {
        if (_playerHp.hpValue > 0 && _IsAttacking == true&&_IsDead==false)
        {
            Debug.Log("Đã tấn  công");
            Vector3 scale = transform.localScale;
            if (_player.position.x < transform.position.x && scale.x > 0 || _player.position.x > transform.position.x && scale.x < 0)
            {
                scale.x *= -1;
                _HpEnemyText.transform.localScale = new Vector3(scale.x, 1, 1);
                transform.localScale = scale;
            }
            _animator.SetBool("IsEnemy4Attack", true);
        }
        else
        {
            if (_EnemyAttackTimeStart > 0)
            {
                _EnemyAttackTimeStart -= Time.deltaTime;
                _animator.SetBool("IsEnemy4Attack", false);
            }
            else if (_playerHp.hpValue <= 0)
            {
                _animator.SetBool("IsEnemy4Run", true);
                _animator.SetBool("IsEnemy4Attack", false);
            }
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
        _animator.SetTrigger("IsEnemy4Idle");
    }
    public void EnemyDead()
    {
        if (hpEmenyValue <= 0 && _IsDead == false)
        {
            StartCoroutine(EnemyDeadOffConllider());
        }
    }
    IEnumerator EnemyDeadOffConllider()
    {
        _boxconllider2D.enabled = false;
        _circleCollider2D.enabled = true;
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Enemy da die");
        _IsDead = true;
        _HpEnemyOff.SetActive(false);
        _rigidbody2.velocity = Vector2.zero;
        _animator.SetTrigger("IsEnemy4Dead");
        Invoke("DestroyEnemy", 2f);
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    public void Enemy4TakeDame(int dame)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Enemy4_IntroAnimation") ||
            stateInfo.IsName("Enemy4_Intro2Animation")) return;
        else if (hpEmenyValue > 0)
        {
            _animator.SetTrigger("IsEnemy4Hurt");
            hpEmenyValue -= dame;
            _EnemyHp.value = hpEmenyValue;
            _HpEnemyText.text = hpEmenyValue.ToString("");
            Invoke("StopHitEnemy", 0.33f);
        }
    }
}
