using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSummon : MonoBehaviour
{
    Animator animator;
    [SerializeField] private GameObject _Enemy;
    public Transform _GateTransform;
    private BoxCollider2D _BoxColliderEnemy;
    private Rigidbody2D _RigidbodyEnemy;
    private GameObject createMonster;
    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
    }
    public void OpenGateSummon()
    {
        animator.SetBool("IsOpenGate", true);
        createMonster = Instantiate(_Enemy, _GateTransform.position, Quaternion.identity);
        createMonster.transform.localScale = new Vector3(2, 2, 2);
        // Lấy các thành phần từ quái vật mới tạo ra
        _BoxColliderEnemy = createMonster.GetComponent<BoxCollider2D>();
        _RigidbodyEnemy = createMonster.GetComponent<Rigidbody2D>();
        if (_BoxColliderEnemy == null || _RigidbodyEnemy == null)
        {
            Debug.LogError("Quái vật mới tạo ra không có BoxCollider2D hoặc Rigidbody2D.");
        }
        _RigidbodyEnemy.velocity = Vector3.down * 1;

    }
}
