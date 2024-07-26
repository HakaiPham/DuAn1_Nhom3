using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _MoveSpeed;
    Rigidbody2D _Rigidbody2;
    BoxCollider2D _Collider2;
    public Animator _Animator2;
    [SerializeField] private float _JumpPower;
    [SerializeField] private float _JumpPowerSkill;
    CircleCollider2D circleCollider;
    [SerializeField] private float _AttackRange;
    public Transform[] _monster;
    [SerializeField] private Transform _TransformAttack;
    public GameObject _Bullet;    //float positionMonster;
     public Slider _HpSlider;
     public Slider _MpSlider;
    public int hpValue;
    [SerializeField] public  TextMeshProUGUI _HpText;
    [SerializeField] public TextMeshProUGUI _MpText;
    public int mpValue;
    public Image _Skill1Image;
    public Image _Skill2Image;
    Monster _HpMonster;
    bool isMonsterinRange;
    float positionMonster;
    [SerializeField] private GameObject _HpItem;
    [SerializeField] private GameObject _MpItem;
    [SerializeField] public TextMeshProUGUI _SlHpText;
    int slHp = 0;
    int slMp = 0;
    [SerializeField] public TextMeshProUGUI _SlMpText;
    bool canUseItem;
    public Transform targetMonster;
    Monster2 _EnemySummon;
    BossTank _BossTank;
    bool isDead;
    private float _TimeAttackStart;
   [SerializeField] private float cooldownAttackTime;
    Enemy2 _enemy2;
    Enemy3 _enemy3;
    EnermyArrow _enemyArrow;
    void Start()
    {
        _Rigidbody2 = GetComponent<Rigidbody2D>();
        _Collider2 = GetComponent<BoxCollider2D>();
        _Animator2 = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        _HpSlider.maxValue = 100;
        hpValue = 100;
        _HpText.text = hpValue.ToString("");
        _MpSlider.maxValue = 100;
        mpValue = 100;
        _MpText.text = mpValue.ToString("");
        _HpMonster = FindFirstObjectByType<Monster>();
        _SlHpText.text = slHp.ToString("");
        _SlMpText.text = slMp.ToString("");
        _EnemySummon = FindObjectOfType<Monster2>();
        _BossTank = FindObjectOfType<BossTank>();
        _enemy2 = FindObjectOfType<Enemy2>();
        isDead = false;
        _TimeAttackStart = 0;
        _enemy3 = FindObjectOfType<Enemy3>();
        _enemyArrow = FindObjectOfType<EnermyArrow>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Jump();
        PlayerAttack();
        Debug.Log("cooldown tan cong con: " + _TimeAttackStart);
        //SkillAttack1();
        if (canUseItem && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (slHp > 0)
            {
                slHp -= 1;
                _Animator2.SetBool("IsHealth", true);
                Invoke("AnimationEatHpFinished", 0.5f);
            }
        }
        else if (canUseItem && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slMp > 0)
            {
                slMp -= 1;
                _Animator2.SetBool("IsMana", true);
                Invoke("AnimationEatMpFinished", 0.5f);
            }
        }
        if (hpValue <= 0 && isDead == false)
        {
            isDead = true;
            Dead();
        }
        PlayerClimp();
    }
    public void AnimationEatHpFinished()
    {
       
            _SlHpText.text = slHp.ToString();
            StartCoroutine(EatHpItem());
            _Animator2.SetBool("IsHealth", false);       
    }
    public void AnimationEatMpFinished()
    {

        _SlMpText.text = slMp.ToString();
        StartCoroutine(EatMpItem());
        _Animator2.SetBool("IsMana", false);

    }
    public void PlayerAttack()
    {
        _TimeAttackStart -= Time.deltaTime;
        isMonsterinRange = false; // quái chưa có vào phạm vi tấn công
        targetMonster = null;
        Debug.Log("Số lượng quái có trong mảng là: " + _monster.Length);
        foreach(Transform monster in _monster) 
        {
            if (monster != null) // Kiểm tra nếu monster không phải là null
            {
                positionMonster = Vector2.Distance(transform.position, monster.position);
                if (positionMonster <= _AttackRange)
                {
                    isMonsterinRange = true;
                    targetMonster = monster; // Lưu lại quái vật trong phạm vi tấn công
                    break; // Thoát khỏi vòng lặp nếu tìm thấy quái vật trong phạm vi
                }
            }
        }
        if (isMonsterinRange==true&&_TimeAttackStart<=0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Đã tấn công thành công");
                _Animator2.SetTrigger("IsAttack");
                AttackMonsterbyNormalAttack(targetMonster);
                _TimeAttackStart = cooldownAttackTime;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.F)&&_MpSlider.value>=10
                    &&_Skill1Image.fillAmount>=1)
                {
                    Debug.Log("Đã tấn công thành công");
                    SkillAttack1();
                    _MpSlider.value -= 10;
                    mpValue -= 10;
                    _MpText.text = mpValue.ToString("");
                    _Skill1Image.fillAmount = 0;
                    //AttackMonsterbySkill1();
                    StartCoroutine(ReLoadSkill1());
                    _TimeAttackStart = cooldownAttackTime;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.R)&&_MpSlider.value>=20
                        &&_Skill2Image.fillAmount>=1)
                    {
                        OnSkill2();
                        Debug.Log("Đã tấn công thành công");
                        _MpSlider.value -= 20;
                        mpValue -= 20;
                        _MpText.text = mpValue.ToString("");
                        Invoke("CreateBullet",0.47f);
                        _Skill2Image.fillAmount = 0;
                        _TimeAttackStart = cooldownAttackTime;
                    }
                }
            }
        }
        else
        {
            if (isMonsterinRange == false&&_TimeAttackStart<=0)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _Animator2.SetTrigger("IsAttack");
                    _TimeAttackStart = cooldownAttackTime;
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F) && _MpSlider.value >= 10
                        && _Skill1Image.fillAmount >= 1)
                    {
                        SkillAttack1();
                        _MpSlider.value -= 10;
                        mpValue -= 10;
                        _MpText.text = mpValue.ToString("");
                        _Skill1Image.fillAmount = 0;
                        StartCoroutine(ReLoadSkill1());
                        _TimeAttackStart = cooldownAttackTime;
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.R) && _MpSlider.value >= 20
                            && _Skill2Image.fillAmount >= 1)
                        {
                            OnSkill2();
                            _MpSlider.value -= 20;
                            mpValue -= 20;
                            _MpText.text = mpValue.ToString("");
                            Invoke("CreateBullet", 0.47f);
                            _Skill2Image.fillAmount = 0;
                            _TimeAttackStart = cooldownAttackTime;
                        }
                    }
                }
            }
        }
        //StopAttack();
    }
    public void TakeDame(int dame)
    {
        if (hpValue >= 0)
        {
            hpValue -= dame;
            _HpSlider.value = hpValue;
            _HpText.text = hpValue.ToString("");
            _Animator2.SetTrigger("IsHurt");
            Invoke("StopHurt", 0.2f);
        }
    }
    public void StopHurt()
    {
        _Animator2.SetTrigger("IsIdle");
    }
    IEnumerator ReLoadSkill1()
    {
        for(int i = 0; i < 10; i++)
        {
            _Skill1Image.fillAmount += 0.1f;
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator ReLoadSkill2()
    {
        for (int i = 0; i < 10; i++)
        {
            _Skill2Image.fillAmount += 0.1f;
            yield return new WaitForSeconds(1f);
        }
    }
    public void CreateBullet()
    {
        var createBullet = Instantiate(_Bullet, _TransformAttack.position, Quaternion.identity);
        if (transform.localScale.x > 0)
        {
            var transformAttack = new Vector2(3f, 0);
            createBullet.transform.localScale = new Vector3(2, 2, 2);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        else if (transform.localScale.x < 0)
        {
            var transformAttack = new Vector2(-3f,0);
            createBullet.transform.localScale = new Vector3(-2, -2, -2);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        StartCoroutine(ReLoadSkill2());
        Destroy(createBullet,2f);

    }
    public void MovePlayer()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        _Animator2.SetBool("IsRun", true);
        var playerposition = transform.position;
        var localscale = transform.localScale;
        if (horizontalInput > 0 || horizontalInput < 0)
        {
            localscale.x = Mathf.Abs(transform.localScale.x) * Mathf.Sign(horizontalInput);
            transform.localScale = localscale;
        }
        if (horizontalInput == 0)
        {
            _Animator2.SetBool("IsRun", false);
        }
        transform.localPosition += new Vector3(horizontalInput, 0, 0) * _MoveSpeed * Time.deltaTime;
    }
    public void Jump()
    {
        if (!_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _Animator2.SetTrigger("IsJump");
                _Rigidbody2.velocity = new Vector2(_Rigidbody2.velocity.x, _JumpPower);
            }
            else
            {
                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    _Animator2.SetBool("IsIdle", true);
                }
            }
        }
    }
    public void Dead()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        _Collider2.enabled = false;//Đảo ngước trạng thái của Conllider bật tắc
            circleCollider.enabled = true;
        _Rigidbody2.gravityScale = 1;
        if (!stateInfo.IsName("Player_DeadAnimation")){
            _Animator2.SetTrigger("IsDead");
            _Animator2.ResetTrigger("IsHurt");
            _Animator2.ResetTrigger("IsIdle");
        }          
    }
    public void SkillAttack1()
    {
        // Bắt đầu animation skill
        _Animator2.SetTrigger("IsSkill1");
        _Rigidbody2.velocity = Vector2.up * _JumpPowerSkill;
    }
    public void OnSkill1End()
    {
        // Animation event khi skill 1 kết thúc
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _Animator2.SetTrigger("IsSkill1_2");
            AttackMonsterbySkill1(targetMonster);
        }
        else
        {
            _Animator2.ResetTrigger("IsSkill1_2");
        }
    }
    public void OnSkill2()
    {
           _Animator2.SetTrigger("IsAttackSkill2");
    }
    public void OnSkill2End()
    {
        _Animator2.SetTrigger("IsIdle");
    }
    public void PlayerClimp()
    {
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Stair")))
        {
            _Rigidbody2.gravityScale = 0;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _Animator2.SetBool("IsClimp", true);
                _Rigidbody2.velocity = Vector2.down * 1;
            }
            else if (!Input.GetKey(KeyCode.DownArrow))
            {
                _Rigidbody2.velocity = Vector2.zero;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _Animator2.SetBool("IsClimp", true);
                _Rigidbody2.velocity = Vector2.up * 1;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                _Rigidbody2.velocity = Vector2.zero;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ItemHp"))
        {
            Destroy(_HpItem);
            slHp += 1;
            _SlHpText.text = slHp.ToString("");
            canUseItem = true;
        }
        if (collision.gameObject.CompareTag("ItemMp"))
        {
            Destroy(_MpItem);
            slMp += 1;
            _SlMpText.text = slMp.ToString("");
            canUseItem = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            _Rigidbody2.gravityScale = 1;
            _Animator2.SetBool("IsClimp", false);
        }
    }
    private void AttackMonsterbyNormalAttack(Transform monster)
    {
        if (monster == null) return;

        _HpMonster = monster.GetComponent<Monster>();
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        _enemyArrow = monster.GetComponent<EnermyArrow>();
        if (positionMonster <= _AttackRange)
        {
            if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
            {
                _HpMonster.TakeDameEnemy(5);
            }
            else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
            {
                _EnemySummon.EnemySummonHitbyPlayerAttackNormal();
            }
            if(_BossTank != null && _BossTank._HpBossTankValue > 0)
            {
                _BossTank.TakeDameBoss(5);
            }
            else
            {
                if (_enemy2 != null && _enemy2.hpEmenyValue > 0) _enemy2.Enemy2TakeDame(5);
            }
            if(_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(5);
            else if (_enemyArrow != null && _enemyArrow.hp > 0) _enemyArrow.TakeDamge(5);
        }
    }
    private void AttackMonsterbySkill1(Transform monster)
    {
        if (monster == null) return;

        _HpMonster = monster.GetComponent<Monster>();
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        if (positionMonster <= _AttackRange)
        {
            if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
            {
                _HpMonster.TakeDameEnemy(20);
            }
            else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
            {
                _EnemySummon.EnemySummonHitbyPlayerSkill1();
            }
            if (_BossTank != null&&_BossTank._HpBossTankValue>0)
            {
                _BossTank.TakeDameBoss(20);
            }
            else
            {
                if (_enemy2 != null&&_enemy2.hpEmenyValue>0) _enemy2.Enemy2TakeDame(20);
            }
            if (_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(20);
        }
    }
    public void AttackMonsterbySkill2(Transform monster)
    {
        if (monster == null) return;
        _HpMonster = monster.GetComponent<Monster>();
        _EnemySummon = monster.GetComponent<Monster2>();
        _BossTank = monster.GetComponent<BossTank>();
        _enemy2 = monster.GetComponent<Enemy2>();
        _enemy3 = monster.GetComponent<Enemy3>();
        if (_HpMonster != null && _HpMonster.hpEmenyValue > 0)
        {
            _HpMonster.TakeDameEnemy(50);
        }
        else if (_EnemySummon != null && _EnemySummon._HpMonsterSummonValue > 0)
        {
            _EnemySummon.EnemySummonHitbyPlayerSkill2();
        }
        if (_BossTank != null)
        {
            _BossTank.TakeDameBoss(50);
        }
        else
        {
            if (_enemy2 != null) _enemy2.Enemy2TakeDame(50);
        }
        if (_enemy3 != null && _enemy3.hpEmenyValue > 0) _enemy3.Enemy3TakeDame(50);
    }
    IEnumerator EatHpItem()
    {
        Debug.Log("--------------");
        for(int i = 0; i < 100; i++)
        {
            if (hpValue>=0&&hpValue <100)
            {
                hpValue += 20;
                _HpText.text = hpValue.ToString("");
                _HpSlider.value = hpValue;
                if(hpValue==100)
                {
                    break;
                }
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
    IEnumerator EatMpItem()
    {
        Debug.Log("--------------");
        for (int i = 0; i < 100; i++)
        {
            if (mpValue >= 0 && mpValue < 100)
            {
                mpValue += 20;
                _MpText.text = mpValue.ToString("");
                _MpSlider.value = mpValue;
                if (mpValue == 100)
                {
                    break;
                }
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}
