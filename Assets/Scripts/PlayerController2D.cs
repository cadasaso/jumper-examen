using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // ===== Movimiento =====
    [Header("Movimiento")]
    public float moveSpeed = 8f;       // Velocidad horizontal con A/D
    private float moveInput;           // -1 (A), 0, 1 (D)

    // ===== Salto & Física =====
    [Header("Salto")]
    private Rigidbody2D rb;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // ===== Ground Check =====
    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    // ===== Coyote Time =====
    [Header("Coyote & Buffer")]
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    // ===== Jump Buffering =====
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    // ===== Double Jump =====
    [Header("Doble salto")]
    public int extraJumps = 1;
    private int extraJumpsValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps; // inicial
    }

    void Update()
    {
        // --- Lectura de movimiento (A / D) ---
        moveInput = 0f;
        if (Input.GetKey(KeyCode.A)) moveInput -= 1f;  // izquierda
        if (Input.GetKey(KeyCode.D)) moveInput += 1f;  // derecha
        // Nota: si mantienes A y D a la vez, queda en 0 (se cancelan)

        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Reset de coyote y doble salto cuando tocamos suelo ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffering: recordar el input unos ms ---
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- Lógica combinada de salto (coyote/buffer/doble) ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f) // prioridad: salto de suelo (con coyote)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumpsValue > 0) // sino, salto aéreo
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumpsValue--;
                jumpBufferCounter = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // --- Aplicar movimiento horizontal ---
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Better Falling / Short Hop ---
        if (rb.linearVelocity.y < 0f)
        {
            // Caída más rápida
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0f && !Input.GetButton("Jump"))
        {
            // Salto corto al soltar el botón
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    // Gizmo para ver el GroundCheck en la Scene
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
