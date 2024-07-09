using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Transform[] _monster;
    Rigidbody2D _rb;
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
    Bullet bullet;
    [SerializeField] private GameObject _HpItem;
    [SerializeField] private GameObject _MpItem;
    [SerializeField] public TextMeshProUGUI _SlHpText;
    int slHp = 0;
    int slMp = 0;
    [SerializeField] public TextMeshProUGUI _SlMpText;
    bool canUseItem;
    void Start()
    {
        _Rigidbody2 = GetComponent<Rigidbody2D>();
        _Collider2 = GetComponent<BoxCollider2D>();
        _Animator2 = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _HpSlider.maxValue = 100;
        hpValue = 100;
        _HpText.text = hpValue.ToString("");
        _MpSlider.maxValue = 100;
        mpValue = 100;
        _MpText.text = mpValue.ToString("");
        _HpMonster = FindFirstObjectByType<Monster>();
        bullet = FindObjectOfType<Bullet>();
        _SlHpText.text = slHp.ToString("");
        _SlMpText.text = slMp.ToString("");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Jump();
        PlayerAttack();
        Hurt();
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
        isMonsterinRange = false; // quái chưa có vào phạm vi tấn công
        foreach(Transform monster in _monster) 
        {
            positionMonster = Vector2.Distance(transform.position, monster.position);
            Debug.Log("Khoảng cách của quái vật đến nhân vật là: " + positionMonster);
            if(positionMonster <= _AttackRange) 
            {
                isMonsterinRange = true;
                break; //Nếu ít hơn 1 quái thì sẽ thoát khỏi vòng lặp
            }
        }
        if (isMonsterinRange==true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Đã tấn công thành công");
                _Animator2.SetTrigger("IsAttack");
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
                    }
                }
            }
        }
        else
        {
            if (isMonsterinRange == false)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _Animator2.SetTrigger("IsAttack");

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
                        }
                    }
                }
            }
        }
        StopAttack();
    }
    public void Hurt()
    {
        if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Monster")))
        {
            if (_HpMonster.hpEmenyValue > 0)
            {
                _Animator2.SetTrigger("IsHurt");
            }
        }
        else
        {
            _Animator2.SetTrigger("IsIdle");
        }
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
            createBullet.transform.localScale = new Vector3(1, 1, 1);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        else if (transform.localScale.x < 0)
        {
            var transformAttack = new Vector2(-3f,0);
            createBullet.transform.localScale = new Vector3(-1, -1, -1);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        StartCoroutine(ReLoadSkill2());
        Destroy(createBullet,2f);

    }
    public void StopAttack()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0); // Lấy thông tin của 
        //Animation
        if(stateInfo.IsName("Player_AttackAnimation")&&stateInfo.normalizedTime >= 1.0f)
        {
            _Animator2.SetTrigger("IsIdle");
        }
        else if (stateInfo.IsName("Player_AttackAnimation") && stateInfo.normalizedTime < 1.0f)
        {
            _Animator2.ResetTrigger("IsIdle"); // Đảm bảo không chuyển về Idle trong khi hoạt ảnh tấn công chưa hoàn thành
        } 
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
        if (playerposition.x <= -8.31f && horizontalInput < 0 || playerposition.x >= 8.25f && horizontalInput > 0)
        {
            return;
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
        _rb.velocity = Vector2.up * _JumpPowerSkill;
    }
    public void OnSkill1End()
    {
        // Animation event khi skill 1 kết thúc
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _Animator2.SetTrigger("IsSkill1_2");
            AttackMonsterbySkill1();
        }
        else
        {
            _Animator2.ResetTrigger("IsSkill1_2");
        }
    }
    public void OnSkill1_2End()
    {
        // Animation event khi skill 1_2 kết thúc
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        if (isMonsterinRange == true && stateInfo.normalizedTime >= 1)
        {
            Debug.Log("Tan cong thanh cong");
            _HpMonster.hpEmenyValue -= 10;
            _HpMonster._HpEnemyText.text = _HpMonster.hpEmenyValue.ToString("");
            _HpMonster._EnemyHp.value -= 10;
            _HpMonster.HitEnemy();
        }
        _HpMonster.StopHitEnemy();
        _Animator2.SetTrigger("IsIdle");

    }   
    public void OnSkill2()
    {
        _Animator2.SetTrigger("IsAttackSkill2");
    }
    public void OnSkill2End()
    {
        _Animator2.SetTrigger("IsIdle");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            if (_HpMonster.hpEmenyValue > 0 && hpValue > 0)
            {
                _HpSlider.value -= 5;
                hpValue -= 5;
                _HpText.text = hpValue.ToString("");
            }
            else
            {
                if (hpValue <= 0) Dead();
            }
        }
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
    public void HpPlayer()
    {
        _HpSlider.value -= 5;
        hpValue -= 5;
        _HpText.text = hpValue.ToString("");
    }
    public void AttackMonsterbyNormalAttack()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        if (positionMonster <= _AttackRange)
        {
            if (_HpMonster.hpEmenyValue > 0)
            {
                _HpMonster.hpEmenyValue -= 5;
                _HpMonster._HpEnemyText.text = _HpMonster.hpEmenyValue.ToString("");
                _HpMonster._EnemyHp.value = _HpMonster.hpEmenyValue;
                _HpMonster.HitEnemy();
            }
        }
        _HpMonster.StopHitEnemy();
    }
    public void AttackMonsterbySkill1()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        if (positionMonster <= _AttackRange)
        {
            Debug.Log("Tan cong thanh cong");
            _HpMonster.hpEmenyValue -= 10;
            _HpMonster._HpEnemyText.text = _HpMonster.hpEmenyValue.ToString("");
            _HpMonster._EnemyHp.value = _HpMonster.hpEmenyValue;
            if (_HpMonster._EnemyHp.value > 0)
            {
                _HpMonster.HitEnemy();
            }
        }
        if (_HpMonster._EnemyHp.value > 0) _HpMonster.StopHitEnemy();
    }
    public void AttackMonsterbySkill2()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0);
        if ((positionMonster <= _AttackRange || positionMonster > _AttackRange))
        {
            _HpMonster.hpEmenyValue -= 20;
            _HpMonster._HpEnemyText.text = _HpMonster.hpEmenyValue.ToString("");
            _HpMonster._EnemyHp.value = _HpMonster.hpEmenyValue;
            if (_HpMonster._EnemyHp.value > 0)
            {
                _HpMonster.HitEnemy();
            }
        }
        if (_HpMonster._EnemyHp.value > 0) _HpMonster.StopHitEnemy();
    }
    public void HurtInRangeAttackMonster()
    {
        if (_HpMonster.hpEmenyValue > 0)
        {
            _Animator2.SetTrigger("IsHurt");
        }
        else
        {
            _Animator2.SetTrigger("IsIdle");
        }
    }
    IEnumerator EatHpItem()
    {
        Debug.Log("--------------");
        for(int i = 0; i < 10; i++)
        {
            if (hpValue>=0&&hpValue <100)
            {
                hpValue += 1;
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
        for (int i = 0; i < 10; i++)
        {
            if (mpValue >= 0 && mpValue < 100)
            {
                mpValue += 1;
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
