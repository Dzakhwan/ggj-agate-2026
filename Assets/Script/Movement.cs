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
    public bool isCeiling = false; // Status di ceiling
    [SerializeField]
    private float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector3 respawnPoint;
    private bool ceilingMode = false; // Mode berjalan di ceiling



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;

    }

    void FixedUpdate()
    {
        MovementPlayer();
    }

    void Update()
    {
        CheckGround();
        CheckCeiling();
        Jump();
        HandleCeilingMode();
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

    void CheckCeiling()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.1f;
        Vector2 direction = Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, groundCheckDistance, groundLayer);
        isCeiling = hit.collider != null;
        Color rayColor = isCeiling ? Color.yellow : Color.gray;
        Debug.DrawRay(origin, direction * groundCheckDistance, rayColor);
    }

    void HandleCeilingMode()
    {
        // Tekan C untuk toggle ceiling mode
        if (Input.GetKeyDown(KeyCode.C) && isCeiling)
        {
            ceilingMode = !ceilingMode;
        }

        // Jika tidak ada ceiling di atas, keluar dari ceiling mode
        if (!isCeiling)
        {
            ceilingMode = false;
        }

        // Update gravity berdasarkan mode
        if (ceilingMode)
        {
            // Mode ceiling: gravity dibalik agar menempel ke atas
            rb.gravityScale = -1f;
        }
        else
        {
            // Mode normal: gravity biasa
            rb.gravityScale = 1f;
        }
    }

    public void Respawn()
    {
        // 1. Pindahkan posisi pemain ke titik respawn
        transform.position = respawnPoint;

        // 2. Reset kecepatan (PENTING! Supaya pas respawn tidak mental karena sisa momentum)
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Player Respawned!");
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
        Debug.Log("Checkpoint Updated!");
    }


}