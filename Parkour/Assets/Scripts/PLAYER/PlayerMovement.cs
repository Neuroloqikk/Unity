using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask boxesLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private BoxCollider2D boxColliderGroundCheck;
    private float wallJumpCooldown;
    private float horizontalInput;
    private int jumpCounter;
    [Header("Inventory Management")]
    public PlayerInventory playerInventory;

    [Header("Layer Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private float wallCheckRadius;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpSound;

    [Header("Coyote time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Wall jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Wall Sliding")]
    [SerializeField] private float wallSlideSpeed;


    private void Awake()
    {
        //Grab references for rigidBody and Animator from object
        body = GetComponent<Rigidbody2D>(); 
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left/right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(5,5,5);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-5,5,5);

        //Set animator parameters
        anim.SetBool("isMoving", horizontalInput != 0);
        anim.SetBool("isGrounded", IsGrounded());

        if (OnWall())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, -wallSlideSpeed);
            anim.SetBool("isSliding", true);
        }
        else
        {
            anim.SetBool("isSliding", false);
            body.gravityScale = 7;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);


            //If Player is jumping, ignore coyoteCounter otherwise it will double jump
            if(IsGrounded() && !IsJumping())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = playerInventory.doubleJumps; //Reset jump counter to extra jumps
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }

        //Jump

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump();
        }
    }

    private void jump()
    {
        //print("CoyoteCounter: " + coyoteCounter + " IsGrounded:"+ IsGrounded());

        //SoundManager.instance.PlaySound(jumpSound);

        if (IsGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            coyoteCounter = 0;
        }
        else if (OnWall())
            WallJump();
        else if (coyoteCounter > 0)
        {
            //If not on the ground and coyote counter bigger than 0 do a normal jump
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
        }
        else if (jumpCounter > 0)
        {
            anim.SetBool("isDoubleJumping", true);
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            jumpCounter--;
            playerInventory.doubleJumps--;
        }
        coyoteCounter = 0;
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    private bool IsGrounded()
    {
        return (Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, 0.1f, boxesLayer));
    }
    private bool OnWall()
    {
        if (IsGrounded()) return false;
        if (horizontalInput < 0.01f)
        {
            if (!(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) return false;
        }
        else
        {
            if (!(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) return false;
        }

        return (Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, wallLayer) || Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, wallLayer));
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && IsGrounded() && !OnWall();
    }

    private bool IsJumping()
    {
        return body.linearVelocity.y > 0;
    }

    private void DoubleJumpOver()
    {
        anim.SetBool("isDoubleJumping", false);
    }

    private void PlayerDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
