using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float _MoveSpeed;
    [SerializeField] private float _JumpPower;
    private Rigidbody2D _Rigidbody2;
    private BoxCollider2D _Collider2;

    void Start()
    {
        _Rigidbody2 = GetComponent<Rigidbody2D>();
        _Collider2 = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        MovePlayer();
        Jump();
    }

    public void MovePlayer()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var playerposition = transform.position;
        if (playerposition.x <= -8.31f && horizontalInput < 0 || playerposition.x >= 8.25f && horizontalInput > 0)
        {
            return;
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
                _Rigidbody2.velocity = new Vector2(_Rigidbody2.velocity.x, _JumpPower);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TrapGai"))
        {
            Debug.Log("Collided with TrapGai!");
        }
    }
}
