using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int coins;

    public int health = 4;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 5f; // using
    public float jumpForce = 3f; // using
    public bool deathState = false;  // not operational yet, need to add menu's for this to be useful
    public Transform groundCheck; // using
    public float groundCheckRadius = 0.2f; // using
    public LayerMask groundLayer; // using
    public Image healthImage; // using
    public TextMeshProUGUI coinText; // using

    private PlayerInput playerIn; // using
    private InputAction moveAction; // using
    private InputAction jumpAction; // using
    private Rigidbody2D rb; // using
    private bool facingRight = true; // using
    private bool isGrounded; // using

    public int extraJumpsValue = 1; // using
    private int extraJumps; // using

    void Start()
    {
        playerIn = GetComponent<PlayerInput>();
        moveAction = playerIn.actions["Move"];
        jumpAction = playerIn.actions["Jump"];
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        extraJumps = extraJumpsValue;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontal = moveInput.x;

        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if ( jumpAction.WasPressedThisFrame() )
        {
            if ( isGrounded )
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if ( extraJumps > 0 )
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumps--;
            }
        }

        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }

        healthImage.fillAmount = health / 5f;
        coinText.text = "Coins: " + coins;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.gameObject.tag == "Damage" )
        {
            health -= 1;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Knockback effect
            StartCoroutine(BlinkRed());
            if (health <= 0)
            {
                Die();
                // Handle player death (e.g., reload scene, show game over screen, etc.)
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
