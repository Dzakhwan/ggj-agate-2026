using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;
    [SerializeField]
    private float jumpForce = 5.0f;
    private float vertical, horizontal;
    public bool isGrounded = false;
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator anim;
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        MovementPlayer();
    }

    void Update()
    {
        CheckGround();
        Jump();
    }

    void MovementPlayer()
    {
        horizontal = Input.GetAxis("Horizontal"); // Nilai -1 (Kiri) sampai 1 (Kanan)

        // --- MODIFIKASI MULAI ---
        // Cek jika pemain mencoba gerak ke KIRI (horizontal < 0) tapi izinnya dicabut
        if (horizontal < 0 && !AbillityManager.instance.canMoveLeft)
        {
            horizontal = 0;
        }


        // Cek jika pemain mencoba gerak ke KANAN (horizontal > 0) tapi izinnya dicabut
        if (horizontal > 0 && !AbillityManager.instance.canMoveRight)
        {
            horizontal = 0;
        }
        // --- MODIFIKASI SELESAI ---

        Vector2 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector2(horizontal * speed, currentVelocity.y);

        anim.SetFloat("Speed", Mathf.Abs(horizontal));

        if (horizontal != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(horizontal),
                1,
                1
            );
        }


    }

    void Jump()
    {
        // --- MODIFIKASI: Tambahkan && AbilityManager.instance.canJump ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && AbillityManager.instance.canJump)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            Debug.Log("Jump");
        }
    }

    void CheckGround()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.1f;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
        Color rayColor = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, direction * groundCheckDistance, rayColor);

        anim.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }

    }


}