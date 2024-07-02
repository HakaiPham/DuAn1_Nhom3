using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _MoveSpeed;
    Rigidbody2D _Rigidbody2;
    BoxCollider2D _Collider2;
    Animator _Animator2;
    [SerializeField] private float _JumpPower;
    CircleCollider2D circleCollider;
    [SerializeField] private float _AttackRange;
    [SerializeField] private Transform[] _monster;
    void Start()
    {
        _Rigidbody2 = GetComponent<Rigidbody2D>();
        _Collider2 = GetComponent<BoxCollider2D>();
        _Animator2 = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Jump();
        PlayerAttack();
        //////Hurt();
        SkillAttack();
        Dead();
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
                _Animator2.SetTrigger("IsAttack");

            }
        }
        StopAttack();
    }
    public void StopAttack()
    {
        AnimatorStateInfo stateInfo = _Animator2.GetCurrentAnimatorStateInfo(0); // Lấy thông tin của 
        //Animation
        if(stateInfo.IsName("Player_AttackAnimation")&&stateInfo.normalizedTime >= 1.0f)
        {
            Debug.Log("Đã tấn công thành công");
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
    public void Hurt()
    {
        if (_Collider2.IsTouchingLayers(LayerMask.GetMask("Boss")))
        {
            _Animator2.SetTrigger("isHurt");
        }
        else
        {
            if (!_Collider2.IsTouchingLayers(LayerMask.GetMask("Boss")))
            {
                _Animator2.SetBool("isidle", true);
            }
        }
    }
    public void Dead()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _Collider2.enabled = !_Collider2.enabled;//Đảo ngước trạng thái của Conllider bật tắc
            circleCollider.enabled = !_Collider2.enabled;
            _Animator2.SetTrigger("IsDead");
        }

    }
    public void SkillAttack()
    {
       
    }
}
