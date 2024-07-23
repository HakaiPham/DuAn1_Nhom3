using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private float _MoveSpeed;
    [SerializeField] private float _JumpPower;
    private Rigidbody2D _Rigidbody2;
    private BoxCollider2D _Collider2;
    private bool _IsGrounded;

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
      
        transform.localPosition += new Vector3(horizontalInput, 0, 0) * _MoveSpeed * Time.deltaTime;
    }

    public void Jump()
    {
        // Kiểm tra xem nhân vật có đang chạm đất không
        _IsGrounded = _Collider2.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (_IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _Rigidbody2.velocity = new Vector2(_Rigidbody2.velocity.x, _JumpPower);
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
