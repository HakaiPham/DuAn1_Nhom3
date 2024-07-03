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
    Animator _Animator2;
    [SerializeField] private float _JumpPower;
    [SerializeField] private float _JumpPowerSkill;
    CircleCollider2D circleCollider;
    [SerializeField] private float _AttackRange;
    [SerializeField] private Transform[] _monster;
    Rigidbody2D _rb;
    [SerializeField] private Transform _TransformAttack;
    public GameObject _Bullet;    //float positionMonster;
    [SerializeField] private Slider _HpSlider;
    [SerializeField] private Slider _MpSlider;
    int hpValue;
    [SerializeField] private TextMeshProUGUI _HpText;
    [SerializeField] private TextMeshProUGUI _MpText;
    int mpValue;
    public Image _Skill1Image;
    public Image _Skill2Image;
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
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Jump();
        PlayerAttack();
        Hurt();
        //SkillAttack1();
    }
    public void PlayerAttack()
    {
        bool isMonsterinRange = false; // quái chưa có vào phạm vi tấn công
        foreach(Transform monster in _monster) 
        {
            float positionMonster = Vector2.Distance(transform.position, monster.position);
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
                    StartCoroutine(ReLoadSkill1());
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.R)&&_MpSlider.value>=20
                        &&_Skill2Image.fillAmount>=1)
                    {
                        OnSkill2();
                        Debug.Log("Đã tấn công thành công");
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
            _Animator2.SetTrigger("IsHurt");
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
            var transformAttack = new Vector2(5f, 0);
            createBullet.transform.localScale = new Vector3(1, 1, 1);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        else if (transform.localScale.x < 0)
        {
            var transformAttack = new Vector2(-5f,0);
            createBullet.transform.localScale = new Vector3(-1, -1, -1);
            createBullet.GetComponent<Rigidbody2D>().velocity = transformAttack;
        }
        StartCoroutine(ReLoadSkill2());
        Destroy(createBullet,5f);

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
        if (!circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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
       
            _Collider2.enabled = !_Collider2.enabled;//Đảo ngước trạng thái của Conllider bật tắc
            circleCollider.enabled = !_Collider2.enabled;
            _Animator2.SetTrigger("IsDead");
        

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
        if (circleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _Animator2.SetTrigger("IsSkill1_2");
        }
        else
        {
            _Animator2.ResetTrigger("IsSkill1_2");
        }
    }

    public void OnSkill1_2End()
    {
        // Animation event khi skill 1_2 kết thúc
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            _HpSlider.value -= 5;
            hpValue -= 5;
            _HpText.text = hpValue.ToString("");
            if (_HpSlider.value <= 0) Dead();
        }
    }
}
