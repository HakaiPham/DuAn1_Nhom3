using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private float _enemyMoveSpeed;//Tốc độ của Enemy
    [SerializeField] private float _Right;//Giới hạn bên phải
    [SerializeField] private float _Left;//Giới hạn bên trái
    [SerializeField] private float _AttackRange;//Phạm vi tấn công
    public Transform _player;//Vị trí của người chơi
    private Animator _animator;
    private bool isMoveLeftOrRight;//Biến xác định xoay sang trái hay sang phải
    private bool _IsAttacking;//Biến xác định Enemy có tấn công không
    float _EnemyAttackTimeStart;//Thời gian ban đầu (mặc đinh là 0)
    [SerializeField] private float _EnemyAttackTime;//Thời gian cooldown tấn công
    Player _playerHp;//Script của player
    public Slider _EnemyHp;//Thanh slider của Enemy
    public TextMeshProUGUI _HpEnemyText;
    public int hpEmenyValue;
    Rigidbody2D _rigidbody2;
    bool _IsDead;//Biến xác định Enemy đã chết chưa
    [SerializeField] private GameObject _HpEnemyOff;
    public TextMeshProUGUI _DameText;
    GameController1 _gameController1;//script của GameController1
    AudioSource audioSource;
    public AudioClip audioClipHitEnemy;
    public AudioClip audioClipDead;
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
        _gameController1 = FindObjectOfType<GameController1>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var distancePlayer = Vector2.Distance(transform.position, _player.position);
        //Khoảng cách của player đến quái
        // Kiểm tra xem người chơi có nằm trong phạm vi tấn công không
        if (distancePlayer <= _AttackRange)//Nếu mà vị trí của người chơi < pv tấn công của quái
        {
            _IsAttacking = true;//Quái được phép tấn công
        }
        else//ngược lại là không
        {
            _IsAttacking = false;
        }

        //Nếu người chơi nằm trong phạm vi tấn công, quái sẽ dừng lại và tấn công

        if (_IsAttacking == false)//Nếu Quái không được phép tấn công
        {
            EnemyMove();//Thì quái được phép di chuyển
        }
        //StopAttack();
        EnemyDead();
    }
    public void EnemyMove()//Quái di chuyển
    {
        var enemyPosition = transform.localPosition;//Lấy vị trí hiện tại của Enemy

        if (enemyPosition.x >= _Right)//Giới hạn bên phải
        {
            isMoveLeftOrRight = false;//Tự động chuyển sang trái
        }
        else if (enemyPosition.x <= _Left)//Giới hạn bên trái
        {
            isMoveLeftOrRight = true;//Tự động chuyển sang phải
        }

        var move = Vector2.right;//Di chuyển sang phải
        _HpEnemyText.transform.localScale = new Vector3(1, 1, 1);
        if (isMoveLeftOrRight == false)//Di chuyển sang trái
        {
            move = Vector2.left;
            _HpEnemyText.transform.localScale = new Vector3(-1, 1, 1);
        }

        var enemyScale = transform.localScale;
        if (enemyScale.x > 0 && isMoveLeftOrRight == false || enemyScale.x < 0 && isMoveLeftOrRight == true)
        {
            //Nếu scale của Enemy là bên phải và bắt đầu di chuyển sang bên trái
            //hoặcNếu scale của Enemy là bên trái và bắt đầu di chuyển sang bên phải
            enemyScale.x *= -1;//đổi hướng xoay
            transform.localScale = enemyScale;
        }
        else
        {
            if (hpEmenyValue <= 0)//Nếu mà máu của Enemy <=0 thì quái sẽ ngừng di chuyển
            {
                return;
            }
        }
        _animator.SetBool("IsEnemy2Run", true);
        _animator.SetBool("IsEnemy2Attack", false);
        _animator.SetBool("IsEnemy2Block", false);
        transform.Translate(move * _enemyMoveSpeed * Time.deltaTime);
    }
    public void SkillMonster()//Kỹ năng của Enemy
    {
        if (_playerHp.hpValue > 0)//Nếu Hp của player >0 thì Enemy mới được phép tấn công
        {
            int randomSkill = Random.Range(1, 3);//Random skill từ 1 -2
            _EnemyAttackTimeStart -= Time.deltaTime;
            if (_EnemyAttackTimeStart <= 0&& _IsAttacking == true)
            {//Nếu mà Quái không nằm trong thời gian cooldown tấn công
                //và được phép tấn công(nằm trong pv tấn công)
                switch (randomSkill)//Randomskill
                {
                    case 1:
                        Vector3 scale = transform.localScale;
                        if (_player.position.x < transform.position.x && scale.x > 0 || _player.position.x > transform.position.x && scale.x < 0)
                        {
                            //Nếu vị trí của player bé hơn vị trí của Enemy và Scale ở bên phải
                            //hoặc Nếu vị trí của player lớn hơn vị trí của Enemy và Scale ở bên trái
                            scale.x *= -1;//Enemy được phép xoay mặt lại ngay
                            //Dòng code dưới để chỉnh cho Text của thanh máu không bị xoay ngược chiều
                            _HpEnemyText.transform.localScale = new Vector3(scale.x, 1, 1);
                            transform.localScale = scale;
                        }
                        _animator.SetBool("IsEnemy2Attack", true);
                        if (_playerHp.hpValue > 0)
                        {
                            Invoke("Hpnv", 0.51f);//Sau 0.51f thì trừ máu player
                        }
                        break;
                    case 2:
                        scale = transform.localScale;
                        if (_player.position.x < transform.position.x && scale.x > 0 || _player.position.x > transform.position.x && scale.x < 0)
                        {
                            scale.x *= -1;
                            _HpEnemyText.transform.localScale = new Vector3(scale.x, 1, 1);
                            transform.localScale = scale;
                        }
                        _animator.SetBool("IsEnemy2Block", true);
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
                _animator.SetBool("IsEnemy2Attack", false);
                _animator.SetBool("IsEnemy2Block", false);
            }
            else if (_playerHp.hpValue <= 0)
            {
                _animator.SetBool("IsEnemy2Run", true);
                _animator.SetBool("IsEnemy2Attack", false);
                _animator.SetBool("IsEnemy2Block", false);
            }
        }
    }
    public void Hpnv()
    {
        if(_playerHp.hpValue > 0)
        {
            _playerHp.TakeDame(5);
        }
        else
        {
            return;
        }
    }
    public void HitEnemy()
    {
        _animator.SetTrigger("IsEnemy2Hurt");
    }
    public void StopHitEnemy()
    {
        _animator.SetTrigger("IsEnemy2Idle");
    }
    public void EnemyDead()
    {
        if (hpEmenyValue <= 0 && _IsDead == false)
        {
            _IsDead = true;
            _HpEnemyOff.SetActive(false);
            _rigidbody2.velocity = Vector2.zero;
            _animator.SetBool("IsEnemy2Run", false);
            audioSource.PlayOneShot(audioClipDead);
            _animator.SetTrigger("IsEnemy2Dead");
            Destroy(gameObject, 2f);
        }
    }
    public void Enemy2TakeDame(int dame)//Hàm nhận dame của Enemy
    {
        bool canAttack = true;//được phép tấn công
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        //Lấy trạng thái của Animation hiện tại
        if (stateInfo.IsName("Enemy2_blockAnimation")||
            (stateInfo.IsName("Enemy2_AttackAnimation") && canAttack == true)) return;
        //Nếu mà Enemy đang trong trạng thái cầm khiên và trong trạng thái tấn công 
        //người chơi thì không thể bị tấn công
        else
        {
            if (hpEmenyValue > 0)//Nếu mà Hp của Enemy >0
            {
                _animator.SetTrigger("IsEnemy2Hurt");
                audioSource.PlayOneShot(audioClipHitEnemy);
                hpEmenyValue -= dame;
                if (hpEmenyValue <= 0) { hpEmenyValue = 0; }
                _gameController1.StartDameText(dame, _DameText,gameObject.transform);
                _EnemyHp.value = hpEmenyValue;
                _HpEnemyText.text = hpEmenyValue.ToString("");
                Invoke("StopHitEnemy", 0.3f);
                canAttack = false;
            }
        }
    }
}
